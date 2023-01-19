# Introduction

*Basic* is an API driven product.
As such all actions available in the web app can be executed via the following API as well.

## Where to start

- Start by using the `/auth` to authenticate yourself.
  The received payload will contain an `accessToken`.
- Send a new request to /clients with a `Authorization` header set to `Bearer ` followed by the accessToken.
