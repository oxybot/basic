import got from "got";

let apiUrl = "https://localhost:7268";
if (process.env.CODESPACES === "true") {
  console.log("Running in Codespaces");
  apiUrl = `https://${process.env.CODESPACE_NAME}-7268.${process.env.GITHUB_CODESPACES_PORT_FORWARDING_DOMAIN}`;
}

console.log("base url: " + apiUrl);

// Authentication
var temp = await got.get(apiUrl + "/definitions").json();
console.log(temp);

var result = await got.post(apiUrl + "/auth", {
  method: "POST",
  json: { username: "demo", password: "iX3vvI7ugmOKkDdYY4v2fQ" },
}).json();
var token = result.accessToken;

// Default client
var client = got.extend({ headers: { authorization: "Bearer " + token } });
console.log("start of execution");

// Create new users
console.log("  - create users");
for (let i = 0; i < 100; i++) {
  await client.post(apiUrl + "/users", {
    json: { username: "user" + i, displayName: "test user " + i },
  });
}
