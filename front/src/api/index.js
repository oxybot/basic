import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { addFatal } from "../Alerts/slice";

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

        reject(reason);
      });
  });
}

const rootApiUrl = "https://localhost:7268";

export function apiUrl(...relative) {
  if (relative.length === 1 && Array.isArray(relative[0])) {
    return new URL(relative[0].join("/"), rootApiUrl);
  } else {
    return new URL(relative.join("/"), rootApiUrl);
  }
}

export function getDefinition(type) {
  return retries(() =>
    fetch(apiUrl("Definitions", type), {
      method: "GET",
      headers: { "content-type": "application/json", accept: "application/json" },
    })
  ).then((response) => response.json());
}

const defaultTransform = (e) => e;
export function useDefinition(type, transform = defaultTransform) {
  const dispatch = useDispatch();
  const [definition, setDefinition] = useState(null);
  useEffect(() => {
    getDefinition(type)
      .then((definition) => transform(definition))
      .then((definition) => setDefinition(definition))
      .catch((err) => {
        console.error("Server error");
        console.log(err);
        dispatch(addFatal("Server error", "Can't retrieve key information for this page."));
      });
  }, [dispatch, type, transform]);

  return definition;
}

export async function apiFetch(url, options) {
  const uri = typeof url === "string" ? apiUrl(url) : url;
  const tokenCookie = await window.cookieStore.get("access-token");
  const token = tokenCookie.value;
  const headers = options.headers || {};
  const connectedOptions = {
    ...options,
    headers: {
      ...headers,
      "content-type": "application/json",
      accept: "application/json",
      authorization: "Bearer " + token,
    },
  };
  return retries(() => {
    return fetch(apiUrl(uri), connectedOptions);
  }).then(async (response) => {
    if (response.ok) {
      const body = await response.json();
      return body;
    } else if (response.status === 400) {
      const body = await response.json();
      throw body;
    } else {
      throw new Error(response.status);
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
  }, [url.toString(), JSON.stringify(options)]);

  return [loading, response];
}
