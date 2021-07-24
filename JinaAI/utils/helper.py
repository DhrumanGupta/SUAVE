import shutil
import sys
import os
import itertools
import csv
from jina import Document
import re

from config import max_docs


def clean_string(s):
    res = re.sub(r'[^\w\s]', '', s)
    res = res.replace("  ", " ")
    return res.lower()


def docs_generator(input_file, num_docs=max_docs):

    with open(input_file, "r") as csv_file:
        reader = csv.reader(csv_file)
        for row in itertools.islice(reader, num_docs):
            with Document() as doc:
                if row[0] != 'name':

                    doc.tags['Name'] = row[0]
                    doc.tags['Category'] = row[3]
                    doc.tags['Link'] = row[2]
                    doc.text = row[0] + row[0] + ' - ' + \
                        row[1] + ' - ' + row[3] + row[3]

                    yield doc


def check_workspace(dir_name, should_exist=False):
    if should_exist:
        if not os.path.isdir(dir_name):
            print(
                f"The directory {dir_name} does not exist. Please index first via `python app.py -t index`"
            )
            sys.exit(1)

    if not should_exist:
        if os.path.isdir(dir_name):
            print("Deleting the old workspace..")
            shutil.rmtree(dir_name)
