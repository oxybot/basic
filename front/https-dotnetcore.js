// This script sets up HTTPS for the application using the ASP.NET Core HTTPS certificate
const fs = require("fs");
const spawn = require("child_process").spawn;
const path = require("path");

const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ""
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv.map((arg) => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : process.env.npm_package_name;

if (!certificateName) {
  console.error(
    "Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly."
  );
  process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Export as PEM/KEY
console.log("Export the dev certificate as pem/key files");
spawn("dotnet", ["dev-certs", "https", "--export-path", certFilePath, "--format", "Pem", "--no-password"], {
  stdio: "inherit",
}).on("close", (code) => {
  if (code !== 0) {
    process.exit(code);
  } else {
    // Update local configuration
    if (!fs.existsSync(".env.development.local")) {
      console.log("Create .env.development.local with path to certificate files");
      fs.writeFileSync(".env.development.local", `SSL_CRT_FILE=${certFilePath}\nSSL_KEY_FILE=${keyFilePath}\n`);
    } else {
      console.log("Update .env.development.local with path to certificate files");
      fs.appendFileSync(".env.development.local", `\nSSL_CRT_FILE=${certFilePath}\nSSL_KEY_FILE=${keyFilePath}\n`);
    }
  }
});
