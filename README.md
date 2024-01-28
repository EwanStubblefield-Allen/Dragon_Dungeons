<br/>
<p align="center">
  <a href="https://github.com/EwanStubblefield-Allen/Dragon_Dungeons">
    <img src="https://freesvg.org/img/Tribal-Dragon-05.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">Dragon Dungeons</h3>

  <p align="center">
    A Dungeons and Dragons character and campaign manager!
    <br/>
    <br/>
    <a href="https://dragons.ewanstubblefield-allen.dev">View Deployment</a>
    .
    <a href="https://github.com/EwanStubblefield-Allen/Dragon_Dungeons/issues">Report Bug</a>
    .
    <a href="https://github.com/EwanStubblefield-Allen/Dragon_Dungeons/issues">Request Feature</a>
  </p>
</p>

![Contributors](https://img.shields.io/github/contributors/EwanStubblefield-Allen/Dragon_Dungeons?color=dark-green) ![Issues](https://img.shields.io/github/issues/EwanStubblefield-Allen/Dragon_Dungeons)

## Table Of Contents

- [Description](#description)
- [Built With](#built-with)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Issues](#issues)
- [License](#license)
- [Authors](#authors)

## Description

This Dungeons and Dragons application allows you to craft unique characters, complete with customizable abilities, backstories, and the option to use AI to create custom images of your character. You can also use the campaign builder to host sessions. Dive into a vast compendium of spells, items, and creatures, and effortlessly integrate them into your campaigns. With all the information in DnD it can be quite overwhelming. Check out the Info section for an intuitive explanation of every subject.

## Built With

This application was built with Vue.js, .Net, Dapper, MySQL.

## Getting Started

To get a local copy up and running follow these simple example steps.

### Prerequisites

- npm

```sh
npm install npm@latest -g
```

### Installation

1. Get a free API Key at [https://platform.openai.com/account/api-keys](https://platform.openai.com/account/api-keys) and [https://cloudinary.com](https://cloudinary.com)

2. Clone the repo

```sh
https://github.com/EwanStubblefield-Allen/Dragon_Dungeons.git
```

3. Restore .Net dependencies

```sh
dotnet restore
```

4. Install NPM packages

```sh
npm install
```

5. Enter your app settings in `server/appsettings.Development.json`

```JSON
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "CONNECTION_STRING": "ENTER YOUR MYSQL CONNECTION STRING",
  "AUTH0_DOMAIN": "ENTER YOUR AUTH0 DOMAIN",
  "AUTH0_AUDIENCE": "ENTER YOU AUTH0 AUDIENCE"
}
```

6. Enter your API in `.client/src/config.js`

```JS
export const OPENAI_API_KEY = 'ENTER YOUR API';
export const IMAGE_API_KEY = 'ENTER YOU API';
export const IMAGE_API_SECRET = 'ENTER YOU API';
```

## Issues

See the [open issues](https://github.com/EwanStubblefield-Allen/Dragon_Dungeons/issues) for a list of proposed features (and known issues).

## License

Distributed under the MIT License. See [LICENSE](https://github.com/EwanStubblefield-Allen/Dragon_Dungeons/blob/main/LICENSE.md) for more information.

## Authors

- **Ewan Stubblefield-Allen** - _Full-Stack Developer_ - [Ewan Stubblefield-Allen](https://github.com/EwanStubblefield-Allen/)
