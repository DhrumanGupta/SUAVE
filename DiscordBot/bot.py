import requests
import json
import discord
import keep_alive
import urllib

client = discord.Client()
baseurl = "http://exploreapiswith.tech/api/"
TOKEN = "ODY4NzcxNDYxODQwNjM3OTUz.YP0geQ._I8Wwi8gLE1dTICsGY3SP_eGPpQ"

@client.event
async def on_message(message):
  if message.author == client.user:
    return
  
  if message.content.lower().startswith('!help'):
    msg = "!list: list of categories\n!category [category]: get apis based on a category\n!use [search word/use case]: get apis on that use case"
    await message.channel.send(msg)
  elif message.content.lower().startswith('!list'):
    try:
      response = json.loads(requests.get(baseurl + "category").text)
      msg = ""
      for i in range(0, len(response)-1):
        msg += response[i] + "\n"
      await message.channel.send(msg)
    except:
      await message.channel.send("I'll just be back. I had some lasagna that didn't suit me last night so I'm AFK.")
  elif message.content.lower().startswith('!category'):
    try:
      query = message.content.replace('!category ', '')
      response = json.loads(requests.get(baseurl + "category/" + urllib.parse.quote(query, safe='')).text)
      msg = ""
      for i in range(min(5,len(response))):
        msg += "Name: " + response[i]['name'] + "\nDescription: " + response[i]['description'] + "\nLink: <" + response[i]['link'] + ">\n--------------------------------\n"
      await message.channel.send(msg)
    except:
      await message.channel.send("Can you please use one of the listed categories in !categorylist? Why are you making my job so hard?")
  elif message.content.lower().startswith('!use'):
    try:
      query = message.content.replace('!use ', '')
      query = urllib.parse.quote(query, safe='')
      response = json.loads(requests.get(baseurl + query).text)
      msg = ""
      for i in range(min(5,len(response))):
        msg += "Name: " + response[i]['name'] + "\nDescription: " + response[i]['description'] + "\nLink: <" + response[i]['link'] + ">\n--------------------------------\n"
      await message.channel.send(msg)
    except: 
      await message.channel.send("I don't know that one, chief. Can you try again with different words or something?")

@client.event
async def on_ready():
       print('Logged in as')
       print(client.user.name)
       print(client.user.id)
       print('------')

keep_alive.keep_alive()

client.run(TOKEN)
