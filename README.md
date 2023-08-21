# Custom Stream Maker

![2023-08-20 (16)](https://github.com/amazeedaizee/CustomStreamMaker/assets/131136866/45bbf8b0-27a8-4892-b079-6e94e300f1fb)

Custom Stream Maker is a program that allows you to make your own Needy Streamer Overload streams using audio, animations and more from the game!

There's also a companion game mod for this program, Custom Stream Loader, which allows you to see your custom streams in action on the game itself!

#### Please note: This program does not come with the assets yourself, you need your own copy of Needy Streamer Overload to preview any assets from the game.

This program also supports custom backgrounds and sprites as well!

**Custom backgrounds** use png and jpg images.

**Custom sprites/animation clips** uses Asset Bundles you make yourself and searches for any valid animation clips to use for previewing. You can also use Asset Bundles from Addressables you create, though you need a valid catalog to do so.

**Note: When using the Asset Bundle option to add custom animations, the program can't tell between Asset Bundles and Addressable Bundles; while this is minor in the program, it might cause a problem when the bundle is read in the companion game mod, so make sure you choose the right option for what kind of bundle you'll use when adding custom animations.**

## Libraries

#### This program uses:
  
- [AssetTools.NET](https://github.com/nesrak1/AssetsTools.NET) <br/>
- [AssetRipper.TextureDecoder](https://github.com/AssetRipper/TextureDecoder) <br/>

For extracting the assets from the game.

Files from the Resources folders are taken from https://github.com/AssetRipper/Tpk.

- [Fmod5Sharp](https://github.com/SamboyCoding/Fmod5Sharp) <br/> 
- [NAudio](https://github.com/naudio/NAudio) <br/>
- [NVorbis](https://github.com/NVorbis/NVorbis) <br/>
- [OggVorbisEncoder](https://github.com/SteveLillis/.NET-Ogg-Vorbis-Encoder) <br/>

For creating audio previews from the game.

- [ImageSharp](https://github.com/SixLabors/ImageSharp) (Apache 2.0 version since this software is visible-source) <br/>

For creating image previews from the game, and from any custom images/sprites uploaded to the program.

## Licensing

**AssetExtractor.cs**, and **CustomAssetExtractor.cs** are licensed under the MIT License. You can find these licenses in their respective files. 

The rest of the code in this repository is All Rights Reserved (c) 2023 amazeedaizee.

## Other 

This program is fan-made and is not associated with xemono and WSS Playground. All properties belong to their respective owners.

Haven't downloaded Needy Streamer Overload yet? 
Get it here: https://store.steampowered.com/app/1451940/NEEDY_STREAMER_OVERLOAD/
