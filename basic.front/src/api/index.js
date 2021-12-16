// Based on: https://stackoverflow.com/a/44577075/17681099
const wait = ms => new Promise(r => setTimeout(r, ms));

export function retries(operation, delay = 300, calls = 2) {
    return new Promise((resolve, reject) => {
        return operation()
            .then(resolve)
            .catch((reason) => {
                if (calls > 0) {
                    return wait(delay)
                        .then(retries.bind(null, operation, delay, calls - 1))
                        .then(resolve)
                        .catch(reject);
                }

                return reject(reason);
            });
    });
}

const rootApiUrl = "https://localhost:7268";

export function apiUrl(...relative) {
    return new URL(relative.join("/"), rootApiUrl);
}

export function getDefinition(type) {
    return retries(() => fetch(apiUrl("Definitions", type), { method: "GET" }))
        .then(response => response.json());
}
