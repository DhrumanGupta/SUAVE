# SUAVE
Search and Use APIs Very Easily

## What is SUAVE?

SUAVE, a backronym for Search and Use APIs Very Easily, does exactly what its name says.

It helps developers search and use APIs by using a web app, a Discord bot or a CLI! For the web app, CLI and Discord bot, users can get a list of categories of APIs in our Database, search for APIs in a certain category, and search for APIs by search terms or use cases. Lastly, users can also use the Jina AI-based search exclusive to the CLI interface to search for APIs.

This allows easy search and traversal of APIs, unbiased searches with no promoted APIs, and the Discord bot allows and enables hackathon teammates to collaboratively search on their own Discord server, which solves all our previous API problems!

## Who Developed SUAVE?

- Vikram
- Garvit
- Dhruman
- Adam

## Where Can I Access SUAVE?
- [Website](http://exploreapiswith.tech/)
- [Discord Bot](https://discord.com/api/oauth2/authorize?client_id=868771461840637953&permissions=8&scope=bot)
- [CLI](https://github.com/DhrumanGupta/SUAVE/releases/tag/v1.0.0-cli)

## The Story Behind SUAVE
### Inspiration
I remember once during a hackathon, I thought that there would probably be an API for some task I wanted to achieve. That API did exist, but traversing through the sea of APIs that was there before me was not that difficult, but rather excruciating.

What made this worse was that because several APIs had paid to be promoted, they rose to the top for my search, even though there were better APIs out there for me.

Even worse (somehow!) was that my teammate had found an API for our purpose already, but because of poor communication and collaboration, and we ended up going down different rabbit holes.

The truth is, each of my teammates has had experiences like this, which we wanted to eliminate.

### Building

- **Building and Deploying an API for the first time**
We’ve all used APIs before, but we thought to have numerous ways to access the database for getting the APIs, an API would be easiest. Thus, we decided to make an API for the first time ever and deploy it. We faced several problems while making and deploying it but after hours of debugging, we finally made it work.

- **Implementing DataStax Astra’s Database and using CQL**
We’d all used SQL to some extent before, but we had to learn an entirely new language for working with DataStax Astra: CQL. CQL was a bit difficult to work with, but we did manage to figure it out. Also, DataStax required an isolated environment for authentication so we had to use a docker container, which we had to host using spare Azure credits. Lastly, the authentication required a zip file, which is quite different from other methods that we've used, which made it much harder to manage secrets and sync them with the team. After all that, it finally worked!

- **Implementing AI-Based Search with Jina**
Jina was incredibly difficult for us to implement. However, we thought the search functionality would marry perfectly with SUAVE, but only one of us knew any AI to begin with, and we needed to look at a lot of previous implementations by other people, documentation and resources to get it to work. After a lot of trial and error, we finally implemented it, albeit only in the CLI (for now!)

- **Getting a big list of APIs**
If you search for a list of APIs online, you’ll definitely find some, but most are on GitHub READMEs which make them hard to scrape. Also, these lists often had some information we didn’t need for our project. Thus, we had to scrape these lists with a Python script to convert a text file to JSON. However, the descriptions in the list I was using weren’t clearly marked which meant that we had to manually mark the descriptions so the Python script would understand. This was extremely time-consuming, but 533 APIs is awesome for just a few hours!

- **Caching**
To stop our server from being overloaded and our Azure credits from being drained, we decided to implement caching on the API. While all of us knew what caching was, we still had to learn how to implement it in an API. After a few errors and some debugging sessions, we finally achieved what we wanted.

## How Can I Help SUAVE?

**At the highest level:** Come up with new catagories, find new APIs
**Manageable by you:** Use our [API](http://exploreapiswith.tech/swagger/index.html) to create your own projects
**For fun:** Share our applications on social media, use our Discord bot & celebrate the number of APIs we have available!
