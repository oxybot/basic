import got from "got";

let apiUrl = "https://localhost:7268";
if (process.env.CODESPACES === "true") {
  console.log("Running in Codespaces");
  apiUrl = `https://${process.env.CODESPACE_NAME}-7268.${process.env.GITHUB_CODESPACES_PORT_FORWARDING_DOMAIN}`;
}

console.log("base url: " + apiUrl);

// Authentication
var result = await got
  .post(apiUrl + "/auth", {
    method: "POST",
    json: { username: "demo", password: process.env.DEMO_PASSWORD ?? "demo" },
    https: { rejectUnauthorized: false },
  })
  .json();
var token = result.accessToken;

// Default api client
var client = got.extend({ headers: { authorization: "Bearer " + token }, https: { rejectUnauthorized: false } });

// Create new users
console.log("  - create users");
for (let i = 0; i < 100; i++) {
  await client.post(apiUrl + "/users", {
    json: { username: "user" + i, displayName: "test user " + i },
  });
}
