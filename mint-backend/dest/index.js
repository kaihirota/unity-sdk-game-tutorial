"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.nextTokenId = void 0;
const express_1 = __importDefault(require("express"));
const providers_1 = require("@ethersproject/providers");
const cors_1 = __importDefault(require("cors"));
const http_1 = __importDefault(require("http"));
const ethers_1 = require("ethers");
const utils_1 = require("ethers/lib/utils");
const keccak256_1 = require("@ethersproject/keccak256");
const strings_1 = require("@ethersproject/strings");
const morgan_1 = __importDefault(require("morgan"));
const dotenv_1 = __importDefault(require("dotenv"));
const sdk_1 = require("@imtbl/sdk");
dotenv_1.default.config();
const app = (0, express_1.default)();
app.use((0, morgan_1.default)("dev")); // Logging
app.use(express_1.default.urlencoded({ extended: false })); // Parse request
app.use(express_1.default.json()); // Handle JSON
app.use((0, cors_1.default)()); // Enable CORS
const router = express_1.default.Router();
// Contract addresses
const foxContractAddress = process.env.FOX_CONTRACT_ADDRESS;
const tokenContractAddress = process.env.TOKEN_CONTRACT_ADDRESS;
// Private key of wallet with minter role
const privateKey = process.env.PRIVATE_KEY;
// Mint Immutable Runner Fox
router.post("/x/mint/fox", async (req, res) => {
    if (!foxContractAddress || !privateKey) {
        res.writeHead(500);
        res.end();
        return;
    }
    try {
        // Set up IMXClient
        const client = new sdk_1.x.IMXClient(sdk_1.x.imxClientConfig({ environment: sdk_1.config.Environment.SANDBOX }));
        // Set up signer
        const provider = (0, providers_1.getDefaultProvider)("sepolia");
        // Connect to wallet with minter role
        const ethSigner = new ethers_1.Wallet(privateKey, provider);
        const tokenId = await (0, exports.nextTokenId)(foxContractAddress, client);
        console.log("Next token ID: ", tokenId);
        // recipient
        const recipient = req.body.to ?? null;
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
        const message = (0, keccak256_1.keccak256)((0, strings_1.toUtf8Bytes)(JSON.stringify(mintRequest)));
        const authSignature = await ethSigner.signMessage((0, utils_1.arrayify)(message));
        mintRequest.auth_signature = authSignature;
        console.log("sender", ethSigner.address, "recipient", recipient, "tokenId", tokenId);
        // Mint
        const mintResponse = await client.mint(ethSigner, mintRequest);
        console.log("Mint response: ", mintResponse);
        try {
            // Transfer to recipient
            const imxProviderConfig = new sdk_1.x.ProviderConfiguration({
                baseConfig: {
                    environment: sdk_1.config.Environment.SANDBOX,
                },
            });
            const starkPrivateKey = await sdk_1.x.generateLegacyStarkPrivateKey(ethSigner);
            const starkSigner = sdk_1.x.createStarkSigner(starkPrivateKey);
            const imxProvider = new sdk_1.x.GenericIMXProvider(imxProviderConfig, ethSigner, starkSigner);
            const result = await imxProvider.transfer({
                type: "ERC721",
                receiver: recipient,
                tokenAddress: foxContractAddress,
                tokenId: mintResponse.results[0].token_id,
            });
            console.log("Transfer result: ", result);
            res.writeHead(200);
            res.end(JSON.stringify(mintResponse.results[0]));
        }
        catch (error) {
            console.log(error);
            res.writeHead(400);
            res.end(JSON.stringify({ message: "Failed to transfer to user" }));
        }
    }
    catch (error) {
        console.log(error);
        res.writeHead(400);
        res.end(JSON.stringify({ message: "Failed to mint to user" }));
    }
});
/**
 * Helper function to get the next token id for a collection
 */
const nextTokenId = async (collectionAddress, imxClient) => {
    try {
        let remaining = 0;
        let cursor;
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
    }
    catch (error) {
        return 0;
    }
};
exports.nextTokenId = nextTokenId;
const gasOverrides = {
    // Use parameter to set tip for EIP1559 transaction (gas fee)
    maxPriorityFeePerGas: 10e9, // 10 Gwei. This must exceed minimum gas fee expectation from the chain
    maxFeePerGas: 15e9, // 15 Gwei
};
const zkEvmProvider = new providers_1.JsonRpcProvider("https://rpc.testnet.immutable.com");
// Mint Immutable Runner Fox
router.post("/mint/fox", async (req, res) => {
    try {
        if (foxContractAddress && privateKey) {
            // Get the address to mint the fox to
            let to = req.body.to ?? null;
            // Get the quantity to mint if specified, default is one
            let quantity = parseInt(req.body.quantity ?? "1");
            // Connect to wallet with minter role
            const signer = new ethers_1.Wallet(privateKey).connect(zkEvmProvider);
            // Specify the function to call
            const abi = ["function mintByQuantity(address to, uint256 quantity)"];
            // Connect contract to the signer
            const contract = new ethers_1.Contract(foxContractAddress, abi, signer);
            // Mints the number of tokens specified
            const tx = await contract.mintByQuantity(to, quantity, gasOverrides);
            await tx.wait();
            res.writeHead(200);
            res.end(JSON.stringify({ message: "Minted foxes" }));
        }
        else {
            res.writeHead(400);
            res.end(JSON.stringify({ message: "Failed to mint" }));
        }
    }
    catch (error) {
        console.log(error);
        res.writeHead(500);
        res.end(JSON.stringify({ message: error }));
    }
});
// Mint Immutable Runner Token
router.post("/mint/token", async (req, res) => {
    try {
        if (tokenContractAddress && privateKey) {
            console.log(req.body);
            // Get the address to mint the token to
            let to = req.body.to ?? null;
            // Get the quantity to mint if specified, default is one
            let quantity = BigInt(req.body.quantity ?? "1");
            // Connect to wallet with minter role
            const signer = new ethers_1.Wallet(privateKey).connect(zkEvmProvider);
            // Specify the function to call
            const abi = ["function mint(address to, uint256 quantity)"];
            // Connect contract to the signer
            const contract = new ethers_1.Contract(tokenContractAddress, abi, signer);
            // Mints the number of tokens specified
            const tx = await contract.mint(to, quantity, gasOverrides);
            await tx.wait();
            res.writeHead(200);
            res.end(JSON.stringify({ message: "Minted ERC20 tokens" }));
        }
        else {
            res.writeHead(400);
            res.end(JSON.stringify({ message: "Failed to mint ERC20 tokens" }));
        }
    }
    catch (error) {
        console.log(error);
        res.writeHead(500);
        res.end(JSON.stringify({ message: error }));
    }
});
app.use("/", router);
http_1.default
    .createServer(app)
    .listen(3000, () => console.log("Listening on port 3000"));
