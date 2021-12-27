import { useEffect, useState } from "react";

// Based on: https://stackoverflow.com/a/44577075/17681099
const wait = (ms) => new Promise((r) => setTimeout(r, ms));

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
  return retries(() => fetch(apiUrl("Definitions", type), { method: "GET" })).then((response) => response.json());
}

export function useDefinition(type) {
  const [definition, setDefinition] = useState(null);
  useEffect(() => {
    getDefinition(type)
      .then((definition) => setDefinition(definition))
      .catch((err) => {
        console.log(err);
      });
  }, [type]);

  return definition;
}

export function apiFetch(url, options) {
  const uri = typeof url === "string" ? apiUrl(url) : url;
  return retries(() => fetch(apiUrl(uri), options)).then((response) => {
    if (response.ok) {
      return response.json();
    } else {
      console.error("Can't retrieve data");
      console.log(response);
      throw new Error("Can't retrieve data");
    }
  });
}

export function useApiFetch(url, options, defaultState = null, transform = (e) => e) {
  const [loading, setLoading] = useState(true);
  const [response, setResponse] = useState(defaultState);
  useEffect(() => {
    apiFetch(url, options)
      .then((response) => {
        setResponse(transform(response));
        setLoading(false);
      })
      .catch((err) => console.log(err));

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [url.toString(), options.toString()]);

  return [loading, response];
}
