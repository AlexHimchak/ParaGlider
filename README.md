# Kinesis Paraglider Challenge Team Hackathon Challenge

Use AWS's Kinesis service to find the optimal time to go Paragliding!  We will be using weather data provided by NOAA with a provided event streamer. 

<image src='https://scijinks.gov/review/noaa/noaa-logo.png' width='300px' alt='NOAA' /><image src='https://images.unsplash.com/photo-1440130266107-787dd24d69d7?ixlib=rb-0.3.5&ixid=eyJhcHBfaWQiOjEyMDd9&s=9b8f0de6077b535709700b6f79ed6db8&auto=format&fit=crop&w=1647&q=80' width='300px' alt='Paragliding' />

## Pre-requisites
The following tools and accounts are required to complete these instructions.

* [Sign-up for an AWS account](https://aws.amazon.com/)
* [Install AWS CLI](https://aws.amazon.com/cli/)
* [NodeJs](https://nodejs.org/en/)
* [Install .NET Core 2.1](https://www.microsoft.com/net/download)

# Level 0

- Create a Kinesis stream in the AWS console
  - Search for `Kinesis` in the services dropdown
  - Click create Kinesis Stream
  - Give it a name and 1 shard
- Clone this repo to get the data streamer application on your machine
- Update kinesis key name in `src/WeatherStationsEvents/appsettings.json`
- Build and run `src/WeatherStationsEvents`
  - Verify from logs in the terminal that events are being generated

# Level 1
- Goal - Create a lambda function to capture the streaming data from the Kinesis stream you just set up
- Here is a ready to go Lambda Function for reading a Kinesis Stream. Make it output the data to Cloudwatch Logs

```javascript
exports.handler = (event, context, callback) => {
    
    for (let i = 0; i < event.Records.length; i++) {
        const eventRecord = JSON.parse(Buffer.from(event.Records[i].kinesis.data, 'base64'));
       //TODO
    }
    
    callback(null, "Hello from Lambda");
};
```

- Make sure to set up this function to trigger off of the Kinesis Stream

# Level 2 - Single Site Notification
- Goal - Find the best time to go fly at Torrey Pines Gliderport. Analyze the streaming data and determine if the weather is good for paragliding
- Conditions - If the conditions at Torrey Pines satisfy these, then it's good to fly!
  - <80% humidity
  - Wind 230 to 290 degrees at 6-12 knots. Gusts below 20
- Trigger a message to Cloudwatch logs to inform when conditions are good to fly

# Level 3 - Multi Site Notification
- Goal - Find the best time to go fly at more than one location
- Use this website to determine the best conditions at another location or locations
  - https://www.sdhgpa.com/sites-guide.html
- Update your lambda function to check the streaming for flying conditions at multiple sites
- Integrate AWS SNS to SMS service to send a notification to yourself
  - Only send one notification per site per day
  - Only notify during daylight hours - 9 AM to 6 PM
- Helpful docs
  - https://docs.aws.amazon.com/sns/latest/dg/SubscribeTopic.html
  - https://docs.aws.amazon.com/sdk-for-javascript/v2/developer-guide/sns-examples-publishing-messages.html
<details><summary>Hints</summary>
<ul>
  <li>You will have to persist the data across lambda invocations in order to know if a notification has already been sent...</li>
</ul>
</details>

# Level 4 - Kinesis Data Analytics
- Goal - Use Kinesis Data Analytics to add a lambda function for pre-processing records
  - Replace `NA` with values of zero on field `barometricPressure`
  - Check output of the processing of the new lambda function
- Helpful docs
  - https://docs.aws.amazon.com/kinesisanalytics/latest/dev/getting-started.html
<details><summary>Hints</summary>
<ul>
  <li>Be very careful with the IAM role for Data Analytics permissions</li>
  <li>Make sure the data streaming application is running when using DA</li>
  <li>Data is base64 encoded!</li>
</ul>
</details>

# BOSS Level - Real Time Analytics with SQL
<p><a target="_blank" rel="noopener noreferrer" href="https://camo.githubusercontent.com/24ee58920381e83562f9780036a8df86ef9dec18/687474703a2f2f696d61676573322e66616e706f702e636f6d2f696d6167652f70686f746f732f31303430303030302f426f777365722d6e696e74656e646f2d76696c6c61696e732d31303430333230332d3530302d3431332e6a7067"><img src="https://camo.githubusercontent.com/24ee58920381e83562f9780036a8df86ef9dec18/687474703a2f2f696d61676573322e66616e706f702e636f6d2f696d6167652f70686f746f732f31303430303030302f426f777365722d6e696e74656e646f2d76696c6c61696e732d31303430333230332d3530302d3431332e6a7067" alt="boss" data-canonical-src="http://images2.fanpop.com/image/photos/10400000/Bowser-nintendo-villains-10403203-500-413.jpg" style="max-width:100%;"></a></p>

- Within the Kinesis Data Analytics Application you created for step 4 use the SQL editor to perform real time analytics on the data
- Create a new SQL query using the templated SQL examples. Use the source data a guide
- Attach the resulting stream of the real time analytics to your original lambda function