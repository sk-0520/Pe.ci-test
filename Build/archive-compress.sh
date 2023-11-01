#!/bin/bash -ue

# 1: 出力ファイル名
# 2: 入力アーカイブファイル名

OUTPUT_FILE=${1}
INPUT_DIR=${2}

CURRENT_DIR="$(pwd)"
pushd "${INPUT_DIR}"
    tar cfv "${OUTPUT_FILE}" ./*
    mv "${OUTPUT_FILE}" "${CURRENT_DIR}"
popd
