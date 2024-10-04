import express, { Request, Response, Express, Router } from "express";
import { getDefaultProvider } from "@ethersproject/providers";
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

// Private key of wallet with minter role
const privateKey = process.env.PRIVATE_KEY;

// Mint Immutable Runner Fox
router.post("/mint/fox", async (req: Request, res: Response) => {
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
    const signer = new Wallet(privateKey, provider);

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
          user: recipient,
          tokens: [
            {
              id: tokenId.toString(),
              blueprint: "onchain-metadata",
              royalties: [
                {
                  recipient: signer.address,
                  percentage: 1,
                },
              ],
            },
          ],
        },
      ],
    };
    const message = keccak256(toUtf8Bytes(JSON.stringify(mintRequest)));
    const authSignature = await signer.signMessage(arrayify(message));
    mintRequest.auth_signature = authSignature;

    // Mint
    const mintResponse = await client.mint(signer, mintRequest);
    console.log("Mint response: ", mintResponse);
    res.writeHead(200);
    res.end(JSON.stringify(mintResponse.results[0]));
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

app.use("/", router);

http
  .createServer(app)
  .listen(3000, () => console.log("Listening on port 3000"));
