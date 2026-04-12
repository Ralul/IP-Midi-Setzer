# Pi-Stops-Input

This program reads inputs from a 16-channel multiplexer connected to a Raspberry Pi. It supports different pin configurations based on the operation mode.

## Prerequisites

- .NET SDK installed
- Environment variable `NETWORK_INTERFACENAME` must be set in your `.env` file or system.

## Usage

You can switch between pin configurations using the `PIN_MODE` environment variable.

### 1. Sequencer Mode
To run the program for the sequencer input, use the following command:

```bash
dotnet run --env PIN_MODE=sequencer
```

**Pin Mapping (Sequencer):**
- S0: 2, S1: 3, S2: 4, S3: 17
- SIG (Input): 27

### 2. Stop Toggles Mode
To run the program for stop inputs, use the following command:

```bash
dotnet run --env PIN_MODE=stop-toggles
```

**Pin Mapping (Stop Toggles):**
- S0: 22, S1: 10, S2: 9, S3: 11
- SIG (Input): 0

## Troubleshooting

Ensure your `.env` file contains the correct network interface name:
```text
NETWORK_INTERFACENAME=eth0
```