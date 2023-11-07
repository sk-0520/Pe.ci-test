#!/bin/bash -ue

# 1: 出力ディレクトリ名
# 2: 入力アーカイブファイル名

OUTPUT_DIR=${1}
INPUT_FILE=${2}

mkdir "${OUTPUT_DIR}"
tar xfv "${INPUT_FILE}" -C "${OUTPUT_DIR}"
