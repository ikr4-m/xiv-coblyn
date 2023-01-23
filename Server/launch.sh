#!/bin/sh
libretranslate --disable-web-ui --port $LIBRE_PORT --update-models --metrics --metrics-auth-token $PROMETHEUS_TOKEN