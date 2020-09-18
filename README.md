# Decisions.Monitoring.Logz.io Integration

Version: 6.x

## Settings

Before using this module you should setup it.<br /> 
Follow **System** > **Settings** , then click **Logz Settings** and **Edit**.<br /> 
You need to set Logz’s URL, a token for sending logs and a token for sending metrics. 

![screenshot of sample](https://github.com/Decisions-Modules/Decisions.Monitoring.Logz.io/blob/master/LogzSettings.png)

In order to get your token for logging:
1) Login at [logz.io](https://logz.io) web site
2) At the upper left corner press settings icon
3) Choose Settings -> General

In order to get your token for metrics:
1) Login at [logz.io](https://logz.io) web site
2) At the upper left corner press settings icon
3) Settings -> Manage Accounts
4) Near the end of page  find "Metrics account plan" section and click on the "Decisions_metric". Some extra information with the token will be expanded

## Logz Data
Here are Logz Kibana screenhots when the both token fields are set to the same token for logs<br />
![screenshot of sample](https://github.com/Decisions-Modules/Decisions.Monitoring.Logz.io/blob/master/KibanaLogs.png)
<br />
![screenshot of sample](https://github.com/Decisions-Modules/Decisions.Monitoring.Logz.io/blob/master/KibanaMetrics.png)

<br />
This is Logz Metrics screenshot<br />

![screenshot of sample](https://github.com/Decisions-Modules/Decisions.Monitoring.Logz.io/blob/master/Metrics.png?raw=true)
