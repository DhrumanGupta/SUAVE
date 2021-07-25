# SUAVE CLI

The Command Line Interface of SUAVE.

<hr>

# Installation

## Manual

### Clone the Repository

```bash
    $ git clone https://github.com/DhrumanGupta/SUAVE.git
    $ cd SUAVE
    $ cd JinaAI
```

### Install the Requirements

```bash
    $ pip install -r requirements.txt
```

### Get the Data from DataStax Astra (Optional - The data has already been fetched and placed in the data directory)

```bash
    $ python utils/get_data.py
```

### Get the Jina Flow Running

-   Indexing

```bash
    $ python app.py -t index
```

-   Querying

```bash
    $ python app.py -t query_restful
```

### Start the CLI

Open a new terminal window in the same directory and

```bash
    $ python cli/cli.py
```

Use arrow keys to navigate and Enter to select!
