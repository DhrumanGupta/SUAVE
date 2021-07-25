import json
import requests
from time import sleep
import os

sauve_logo = """

                `````````````````                 
            ``````````````````````````            
         ````````````````````````````````         
       ```````````.:::-````````````````````       
     ````````````.+hhhy:.````````````````````     
    ``````.+so/+syyyssyyys++ss/.``````````````    
   ``````.shhhyo/-.````.-/syhhh+```````````````   
  ````````:hy+.````````````-shy.````````````````  
 ````````.yy/```````````````.oho````````````````` 
```````/+sho`````````````````.yho+:```````````````
```````yhhh/``````````````````ohhho```````````````
```````+syh+`````````````````.shyo:```````````````
`````````-yy:````````````````+hs.`````````````````
``````````:hy/.````````````.oyy.``````````````````
`````````.shhhy+-.``````.:oyhhhs:`````````````````
``````````-oys+oyysooosyyyooyhhhhs:.``````````````
 ```````````.```.-ohhhy/.```-+yhhhhs/.``````````` 
  ````````````````.////```````.+yhhhhy/.````````  
   `````````````````````````````.+yhhhy:```````   
    ``````````````````````````````./s/.```````    
     ````````````````````````````````````````     
       ````````````````````````````````````       
         ````````````````````````````````         
            ``````````````````````````            
                 ````````````````                 
"""

categories = ["Weather", "Entertainment", "Sports", "Social Media", "Shopping", "Science and Maths", "Music", "Location", "Development", "Currency", "Calendar", "Business",
              "Books", "Art and Design", "Anti-Malware", "Anime", "Animals"]

data_dir = os.path.abspath(os.path.dirname(__file__) + "/../data/apis.json")

data = json.load(open(data_dir))


def gen_category_apis(category):
    api_list = []
    for x in data:
        if x['category'] == category:
            api_list.append(x)
    return api_list


def display_api(api):
    underlined_name = "\033[4m" + api["name"] + "\033[0m"
    print(underlined_name + " - " + api["description"] +
          "\nAccess at " + api["link"])


def display_apis(api_list):
    for api in api_list:
        display_api(api)
        print("------------------\n")
        sleep(0.05)


def get_api(search_term):
    headers = {
        'Content-Type': 'application/json',
    }

    data = '{"top_k":1,"mode":"search","data":["' + search_term + '"]}'

    response = requests.post(
        'http://0.0.0.0:45678/search', headers=headers, data=data)

    response = response.text[response.text.find(
        "tags")+6:response.text.find("embedding")-2]

    tags = response[response.find("{")+1: response.find("}")]
    tags = tags.replace('"', '')
    tags = tags.split(",")

    for tag in tags:
        if "Name:" in tag:
            name = tag.replace("Name:", "")
        elif "Category:" in tag:
            category = tag.replace("Category:", "")
        else:
            link = tag.replace("Link:", "")

    description = response[response.find("text")+7:-1].split(" - ")[-2]

    return {
        "name": name,
        "category": category,
        "description": description,
        "link": link
    }
