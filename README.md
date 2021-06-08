# Azure Functions Study 🧠 
This projects consists in three Azure functions that have the below actions:

- **OnPaymentReceived**: Receives an http request and send it to a Azure Queue and save in a azure sql. ✉️
- **GenerateLicenseFile**: Listen to a queue and when an new message comes, process it to a file and send to a blob storage. 📁
- **EmailLicenseFile**: Listen to a blob storage and when comes an new file, process it and send to the e-mail of the request. 🔚

This project was made for study porposes only, feel free to try it out.
