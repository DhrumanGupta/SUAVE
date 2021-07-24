import json
import requests
import pandas as pd

baseurl = "https://exploreapis.azurewebsites.net/api/"


categories = json.loads(requests.get(
    "https://exploreapis.azurewebsites.net/api/category").text)


def get_category_api(category_name=None):
    category_apis = json.loads(requests.get(
        baseurl + "category/" + category_name).text)
    return category_apis


api_list = []
for category in categories:

    api = get_category_api(category)
    api_list += api

with open(r"data/apis.json", "x") as f:
    json.dump(api_list, f)


json_file = pd.read_json(r"data/apis.json")
json_file.to_csv(r"data/apis.csv", index=False)
