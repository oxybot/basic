// const MailDev = require('maildev')

// const maildev = new MailDev({
//   smtp: 1025 // incoming SMTP port - default is 1025
// })

// maildev.listen(function(err) {
//   console.log('We can now sent emails to port 1025!')
// })

// // Print new emails to the console as they come in
// maildev.on('new', function(email){
//   console.log('Received new email with subject: %s', email.subject)
// })

// // Get all emails
// maildev.getAllEmail(function(err, emails){
//   if (err) return console.log(err)
//   console.log('There are %s emails', emails.length)
// })