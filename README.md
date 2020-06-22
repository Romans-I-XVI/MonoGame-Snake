This repository is public for convenience and for collaboration with authorized individuals, but is not licensed for redistribution in whole or in part. If you have any questions or would like to contribute to this project contact me at contact@romansixvigaming.com

### Support
First off, while I fully encourage building and tinkering with this game yourself via this repository the game is also available for purchase at the following locations. If you would like to support me and my projects your purchase, reviews, etc. on those storefronts would be very much appreciated. Thanks!
* [Steam](https://store.steampowered.com/app/1332800) (Windows/Mac/Linux)
* [Microsoft Store](https://www.microsoft.com/en-us/p/snake-the-game/9n5fthk1blvb) (Xbox One/Windows 10)
* [FireTV](https://www.amazon.com/dp/B089RR8YVB)
* [Roku](https://channelstore.roku.com/details/66795/snake)


# Snake
This is the MonoGame port of Snake which was originally released on the Roku. It has been one of my most successful games, for which I am quite thankful. Snake is of course an old video game concept which has been [recreated many times](https://en.wikipedia.org/wiki/Snake_(video_game_genre)). I like to think the most unique features of this version are first off the way movement works. In this implementation there is no grid for handling movement and collisions as many implementations do. Instead it goes with a more modern sprite/collider based approach, and handles movement with an array of positions following the head sprite movements. Secondly there is the obvious extra of having levels and portals which are somewhat unique to this game.

#### Pre-Requisits
* [MonoGame 3.7.1](https://community.monogame.net/t/monogame-3-7-1-release/11173)
* [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

#### Building
```
git clone https://github.com/Romans-I-XVI/MonoGame-Snake.git
cd MonoGame-Snake
git submodule update --init --recursive
dotnet run --project Snake/Snake.csproj
```
These instructions are of course just the minimum to getting a build running on your computer. From here you can proceed to open the solution in your favorite IDE (I use Rider in Linux, though for most this will probably be Visual Studio) to make changes. These instructions are for the PC version of the game and there may be additional steps to getting other platforms up and running (such as running the UWP version on the Xbox One).

#### Note For Beginners
In my opinion the best places to start tinkering around with changing the game would be in altering level designs and altering theme images. Doing this only involves modifying JSON data or images respectively and doesn't even require digging in to the code.


Enjoy :)
