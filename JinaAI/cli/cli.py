from unicodedata import category
import inquirer
from inquirer.themes import GreenPassion
from time import sleep
from helper import sauve_logo, categories, gen_category_apis, display_apis, display_api, get_api
from termcolor import colored, cprint
import os
import random


def clear():
    os.system('cls' if os.name == 'nt' else 'clear')


clear()
sleep(3)


def type_text(text):
    for c in text:
        print(c, end="", flush=True)
        sleep(0.02)


type_text("Welcome to SUAVE!\n")
sleep(1)


def print_logo():
    for char in sauve_logo:
        if char == '`':
            text = colored('`', 'grey', attrs=['dark'])
            print(text, end='')
        elif char == '\n':
            print(char, end='')
        else:
            text = colored(char, 'blue')
            print(text, end='')
    print()

    print(9 * " " + "Search and Use APIs Very Easily!" + 9 * " ")

    print()
    print()

    sleep(1)


while True:
    print_logo()
    starting_questions = [
        inquirer.List(
            "answer",
            message="Menu",
            choices=["About SUAVE", "Search APIs", "More", "Quit"],
        ),
    ]

    answers = inquirer.prompt(starting_questions, theme=GreenPassion())

    if answers['answer'] == 'About SUAVE':
        clear()
        type_text("\nSUAVE, short for Search and Use APIs Very Easily. SUAVE helps you to easily find the API you are looking for from its database of hundreds of APIs!\n")
        sleep(1)

    elif answers['answer'] == 'Search APIs':
        while True:
            print()
            search_questions = [
                inquirer.List(
                    "answer",
                    message="Search ",
                    choices=["APIs from Categories",
                             "APIs from your Usecase", "Back to Main Menu"],
                )
            ]

            search_answers = inquirer.prompt(
                search_questions, theme=GreenPassion())
            if search_answers['answer'] == "APIs from Categories":
                type_text(
                    "\nSUAVE API Categories::\n\n")

                category_options = [
                    inquirer.List(
                        "category",
                        message="Category",
                        choices=list(map(lambda x: "•  "+x, categories)),
                    )
                ]

                category_answers = inquirer.prompt(
                    category_options, theme=GreenPassion()
                )

                category_apis = gen_category_apis(
                    category_answers["category"][3:])

                clear()
                print("\n\n")
                display_apis(category_apis)
                input("Press Enter to Continue")

            elif search_answers['answer'] == "APIs from your Usecase":
                print("\n")

                usecase_options = [
                    inquirer.Text(
                        "usecase",
                        message="What do you want to use an API for?",)
                ]

                usecase_answer = inquirer.prompt(
                    usecase_options, theme=GreenPassion()
                )

                usecase = usecase_answer["usecase"]
                type_text(
                    "\nSearching APIs for your usecase.....\n\nAPI Found!\n")
                display_api(get_api(usecase))
                print("\n")

            else:
                clear()
                break

    elif answers["answer"] == "Quit":
        print()
        print()
        type_text("Successfully Exited The SUAVE CLI.\n")
        exit(0)

    elif answers["answer"] == "More":
        type_text("SUAVE has multiple frontend endpoints. One of them is the CLI you are using right now.\nThe others can be found at http://exploreapiswith.tech/ or http://exploreapiswith.us/")
        print()
