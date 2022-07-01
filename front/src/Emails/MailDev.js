const express = require('express')
const proxyMiddleware = require('http-proxy-middleware')
const MailDev = require('maildev')
const app = express()

// some business with the existing app

// Define a route for the base path
const maildev = new MailDev({
  basePathname: '/maildev'
})

// Maildev now running on localhost:1080/maildev
maildev.listen(function (err) {
  console.log('We can now sent emails to port 1025!')
})

// proxy all maildev requests to the maildev app
const proxy = proxyMiddleware('/maildev', {
  target: `http://localhost:1080`,
  ws: true,
})

// Maildev available at the specified route '/maildev'
app.use(proxy)