import express, { Request, Response, Express, Router } from "express";
import { getDefaultProvider, JsonRpcProvider } from "@ethersproject/providers";
import cors from "cors";
import http from "http";
import { Wallet, Contract, ethers } from "ethers";
import { arrayify } from "ethers/lib/utils";
import { keccak256 } from "@ethersproject/keccak256";
import { toUtf8Bytes } from "@ethersproject/strings";
import morgan from "morgan";
import dotenv from "dotenv";
import { x, config } from "@imtbl/sdk";

dotenv.config();

const app: Express = express();
app.use(morgan("dev")); // Logging
app.use(express.urlencoded({ extended: false })); // Parse request
app.use(express.json()); // Handle JSON
app.use(cors()); // Enable CORS
const router: Router = express.Router();

// Contract addresses
const foxContractAddress = process.env.FOX_CONTRACT_ADDRESS;
const tokenContractAddress = process.env.TOKEN_CONTRACT_ADDRESS;

// Private key of wallet with minter role
const privateKey = process.env.PRIVATE_KEY;

// Mint Immutable Runner Fox
router.post("/x/mint/fox", async (req: Request, res: Response) => {
  if (!foxContractAddress || !privateKey) {
    res.writeHead(500);
    res.end();
    return;
  }

  try {
    // Set up IMXClient
    const client = new x.IMXClient(
      x.imxClientConfig({ environment: config.Environment.SANDBOX })
    );

    // Set up signer
    const provider = getDefaultProvider("sepolia");

    // Connect to wallet with minter role
    const ethSigner = new Wallet(privateKey, provider);

    const tokenId = await nextTokenId(foxContractAddress, client);
    console.log("Next token ID: ", tokenId);

    // recipient
    const recipient: string = req.body.to ?? null;

    // Set up request
    let mintRequest = {
      auth_signature: "", // This will be filled in later
      contract_address: foxContractAddress,
      users: [
        {
          user: ethSigner.address,
          tokens: [
            {
              id: tokenId.toString(),
              blueprint: "onchain-metadata",
              royalties: [
                {
                  recipient: ethSigner.address,
                  percentage: 1,
                },
              ],
            },
          ],
        },
      ],
    };
    const message = keccak256(toUtf8Bytes(JSON.stringify(mintRequest)));
    const authSignature = await ethSigner.signMessage(arrayify(message));
    mintRequest.auth_signature = authSignature;

    console.log(
      "sender",
      ethSigner.address,
      "recipient",
      recipient,
      "tokenId",
      tokenId
    );

    // Mint
    const mintResponse = await client.mint(ethSigner, mintRequest);
    console.log("Mint response: ", mintResponse);

    try {
      // Transfer to recipient
      const imxProviderConfig = new x.ProviderConfiguration({
        baseConfig: {
          environment: config.Environment.SANDBOX,
        },
      });
      const starkPrivateKey = await x.generateLegacyStarkPrivateKey(ethSigner);
      const starkSigner = x.createStarkSigner(starkPrivateKey);
      const imxProvider = new x.GenericIMXProvider(
        imxProviderConfig,
        ethSigner,
        starkSigner
      );
      const result = await imxProvider.transfer({
        type: "ERC721",
        receiver: recipient,
        tokenAddress: foxContractAddress,
        tokenId: mintResponse.results[0].token_id,
      });
      console.log("Transfer result: ", result);

      res.writeHead(200);
      res.end(JSON.stringify(mintResponse.results[0]));
    } catch (error) {
      console.log(error);
      res.writeHead(400);
      res.end(JSON.stringify({ message: "Failed to transfer to user" }));
    }
  } catch (error) {
    console.log(error);
    res.writeHead(400);
    res.end(JSON.stringify({ message: "Failed to mint to user" }));
  }
});

/**
 * Helper function to get the next token id for a collection
 */
export const nextTokenId = async (
  collectionAddress: string,
  imxClient: x.IMXClient
) => {
  try {
    let remaining = 0;
    let cursor: string | undefined;
    let tokenId = 0;

    do {
      // eslint-disable-next-line no-await-in-loop
      const assets = await imxClient.listAssets({
        collection: collectionAddress,
        cursor,
      });
      remaining = assets.remaining;
      cursor = assets.cursor;

      for (const asset of assets.result) {
        const id = parseInt(asset.token_id, 10);
        if (id > tokenId) {
          tokenId = id;
        }
      }
    } while (remaining > 0);

    return tokenId + 1;
  } catch (error) {
    return 0;
  }
};

const gasOverrides = {
  // Use parameter to set tip for EIP1559 transaction (gas fee)
  maxPriorityFeePerGas: 10e9, // 10 Gwei. This must exceed minimum gas fee expectation from the chain
  maxFeePerGas: 15e9, // 15 Gwei
};
const zkEvmProvider = new JsonRpcProvider("https://rpc.testnet.immutable.com");

// Mint Immutable Runner Fox
router.post("/mint/fox", async (req: Request, res: Response) => {
  console.log(req.body);
  try {
    if (foxContractAddress && privateKey) {
      // Get the address to mint the fox to
      let to: string = req.body.to ?? null;
      // Get the quantity to mint if specified, default is one
      let quantity = parseInt(req.body.quantity ?? "1");

      // Connect to wallet with minter role
      const signer = new Wallet(privateKey).connect(zkEvmProvider);

      // Specify the function to call
      const abi = ["function mintByQuantity(address to, uint256 quantity)"];
      // Connect contract to the signer
      const contract = new Contract(foxContractAddress, abi, signer);

      // Mints the number of tokens specified
      const tx = await contract.mintByQuantity(to, quantity, gasOverrides);
      await tx.wait();

      res.writeHead(200);
      res.end(JSON.stringify({ message: "Minted foxes" }));
    } else {
      res.writeHead(400);
      res.end(JSON.stringify({ message: "Failed to mint" }));
    }
  } catch (error) {
    console.log(error);
    res.writeHead(500);
    res.end(JSON.stringify({ message: error }));
  }
});

// Mint Immutable Runner Token
router.post("/mint/token", async (req: Request, res: Response) => {
  console.log(req.body);
  try {
    if (tokenContractAddress && privateKey) {
      // Get the address to mint the token to
      let to: string = req.body.to ?? null;
      // Get the quantity to mint if specified, default is one
      let quantity = BigInt(req.body.quantity ?? "1");

      // Connect to wallet with minter role
      const signer = new Wallet(privateKey).connect(zkEvmProvider);

      // Specify the function to call
      const abi = ["function mint(address to, uint256 quantity)"];
      // Connect contract to the signer
      const contract = new Contract(tokenContractAddress, abi, signer);

      // Mints the number of tokens specified
      const tx = await contract.mint(to, quantity, gasOverrides);
      await tx.wait();

      res.writeHead(200);
      res.end(JSON.stringify({ message: "Minted ERC20 tokens" }));
    } else {
      res.writeHead(400);
      res.end(JSON.stringify({ message: "Failed to mint ERC20 tokens" }));
    }
  } catch (error) {
    console.log(error);
    res.writeHead(500);
    res.end(JSON.stringify({ message: error }));
  }
});

app.use("/", router);

http
  .createServer(app)
  .listen(3000, () => console.log("Listening on port 3000"));
