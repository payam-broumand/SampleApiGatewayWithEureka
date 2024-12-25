# SampleApiWithEureka

Sample implement ApiGateway with Yarp
In this sample we create two simple rest api service (category and course services)
.
- First of all we register both created services in service registery using Eureka
We running Steeltoe.Eureka from docker image. if you have not Eureka docker image run this command :

"docker run --publish 8761:8761 steeltoeoss/eureka-server"

- add following config to the both created service appsetting files
- in this confnig we must using different application name in one the services
- the config must be added in appsetting.json file write in the below :
-   "spring": {
    "application": {
      "name": "EurekaRegisterExample" // service name that must be unique for every services
    }
  },
  "eureka": {
    "client": {
      "serviceUrl": "http://localhost:8761/eureka/",
      "shouldFetchRegistry": "false", // change this config to reue when you want to add application for service discovery
      "shouldRegisterWithEureka": true, // when you want to register new service in eureka service registery
      "validateCertificates": false
    },
    "instance": {
      "port": "8080",
      "ipAddress": "localhost",
      "preferIpAddress": true
    }
  }

- adding this config to the program file for one of the applications that's we want to register it in eureka. 
    "builder.Services.AddDiscoveryClient(builder.Configuration)"

- running Eureka image from docker
- goto http://localhost:8761/ address you can see registered services in apps instances section
- after that we create new application for service discovery that named "ServiceRegistery" in sample project
- add below config to the appsettings.json file for service discovery
- "spring": {
    "application": {
      "name": "EurekaServiceRegistery"
    }
  },
  "eureka": {
    "client": {
      "serviceUrl": "http://localhost:8761/eureka/",
      "shouldFetchRegistry": "true", // this config must be true to tell eureka this app using for discovery other registered services
      "shouldRegisterWithEureka": false,
      "validateCertificates": false
    },
    "instance": { }
  }

- adding ApiGateway configuration with write two classes that this classes implement IProxyConfig and IProxyConfigProvider interdaces 
- IproxyConfig holds that's config for defining Routes and Clusters
- IProxyConfigProvider using routes and clusters config in IproxyConfig for routing between services registered in eureka by input request and routing on requests than Redirect the request to the appropriate service
- Finaly creating client project in the Razor page sample and sending all requeests to the ApiGateway end point and ApiGateway app handle requests and redirect client to the correct web service
- in client project we created sample project for manaing Categories and Courses created for the which one of the categories

# Server/Client Side Service Registery/Discovery
In this sample in the ServiceRegistery project that as ApiGateway application because the itself attemp to disvocer services registered in eureka service registery it knonw as Client Side Registery and other hand the client refers to the ApiGateway project and it know the ApiGateway project as central server that knows the registered services routes it's knonwn as Server Side Registery