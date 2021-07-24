__copyright__ = "Copyright (c) 2021 Jina AI Limited. All rights reserved."
__license__ = "Apache-2.0"

import click
from config import max_docs, datafile, port, workspace_dir, model

from Flows.executors import DiskIndexer
from utils.helper import docs_generator, check_workspace
from jina import Flow

encoder = 'jinahub://TransformerTorchEncoder'
indexer = DiskIndexer


def index(num_docs=max_docs):
    flow = (
        Flow()
        .add(
            uses=encoder,
            pretrained_model_name_or_path=model,
            name="encoder",
            max_length=50,
        )
        .add(uses=indexer, workspace=workspace_dir, name="indexer", dump_path=workspace_dir, override_with={"index_file_name": "index.json"})
    )

    with flow:
        flow.post(
            on="/index",
            inputs=docs_generator(input_file=datafile, num_docs=num_docs),
            request_size=64,
            read_mode="r",
        )


def query_restful():
    flow = (
        Flow()
        .add(
            uses=encoder,
            pretrained_model_name_or_path=model,
            name="encoder",
            max_length=50,
        )
        .add(uses=indexer, workspace=workspace_dir, name="indexer", dump_path=workspace_dir, override_with={"index_file_name": "index.json"})
    )

    with flow:
        flow.protocol = "http"
        flow.port_expose = port
        flow.block()


@click.command()
@click.option(
    "--task",
    "-t",
    type=click.Choice(["index", "query_restful"], case_sensitive=False),
)
@click.option("--num_docs", "-n", default=max_docs)
def main(task, num_docs):
    if task == "index":
        check_workspace(
            dir_name=workspace_dir, should_exist=False)
        index(num_docs=num_docs)

    if task == "query_restful":
        check_workspace(dir_name=workspace_dir, should_exist=True)
        query_restful()


if __name__ == "__main__":
    main()
