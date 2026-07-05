#!/usr/bin/env bash
set -euo pipefail

# Publishes and installs systemd services that autostart the three
# IP-Midi-Setzer programs on boot:
#   - IP-Midi-Setzer      the MIDI bridge (stops + sequencer combination logic)
#   - Pi-Sequencer-Input  reads the sequencer multiplexer over GPIO
#   - Pi-Stops-Input      reads the stop-toggle multiplexer over GPIO
#
# Run this ON THE RASPBERRY PI, from a checkout of this repo, e.g.:
#   sudo ./scripts/install-autostart.sh
#
# Re-run it any time after pulling new code to rebuild and restart the services.
#
# Configure via environment variables (all optional):
#   RUN_USER     user the services run as              (default: the user who
#                invoked sudo, e.g. `sudo ./install-autostart.sh` as nepomuk
#                runs the services as nepomuk; falls back to "pi")
#   DOTNET_RID   .NET runtime identifier to publish for (default: linux-arm64;
#                use linux-arm for a 32-bit Raspberry Pi OS / older Pi)
#   INSTALL_DIR  where published binaries are placed    (default: /opt/ip-midi-setzer)

REPO_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
RUN_USER="${RUN_USER:-${SUDO_USER:-pi}}"
DOTNET_RID="${DOTNET_RID:-linux-arm64}"
INSTALL_DIR="${INSTALL_DIR:-/opt/ip-midi-setzer}"
ENV_FILE="$REPO_DIR/.env"

if [[ $EUID -ne 0 ]]; then
  echo "Run this with sudo (it installs systemd units and writes to $INSTALL_DIR)." >&2
  exit 1
fi

if ! command -v dotnet >/dev/null; then
  echo "dotnet SDK not found on PATH. Install the .NET SDK on the Pi first." >&2
  exit 1
fi

# service-name -> "csproj-path:executable-name"
declare -A PROJECTS=(
  [ip-midi-setzer]="IP-Midi-Setzer/IP-Midi-Setzer.csproj:IP-Midi-Setzer"
  [pi-sequencer-input]="Pi-Sequencer-Input/Pi-Sequencer-Input.csproj:Pi-Sequencer-Input"
  [pi-stops-input]="Pi-Stops-Input/Pi-Stops-Input.csproj:Pi-Stops-Input"
)

mkdir -p "$INSTALL_DIR"

for service_name in "${!PROJECTS[@]}"; do
  IFS=":" read -r csproj exe_name <<< "${PROJECTS[$service_name]}"
  out_dir="$INSTALL_DIR/$service_name"

  echo "==> Publishing $csproj -> $out_dir"
  dotnet publish "$REPO_DIR/$csproj" \
    -c Release \
    -r "$DOTNET_RID" \
    --self-contained true \
    -p:PublishSingleFile=true \
    -o "$out_dir"

  chown -R "$RUN_USER":"$RUN_USER" "$out_dir"

  unit_path="/etc/systemd/system/$service_name.service"
  echo "==> Writing $unit_path"
  cat > "$unit_path" <<EOF
[Unit]
Description=$service_name (IP-Midi-Setzer)
After=network-online.target
Wants=network-online.target

[Service]
Type=simple
User=$RUN_USER
SupplementaryGroups=gpio
WorkingDirectory=$out_dir
ExecStart=$out_dir/$exe_name
EnvironmentFile=-$ENV_FILE
Restart=on-failure
RestartSec=2

[Install]
WantedBy=multi-user.target
EOF
done

echo "==> Reloading systemd and enabling services"
systemctl daemon-reload
for service_name in "${!PROJECTS[@]}"; do
  systemctl enable --now "$service_name.service"
done

echo
echo "Done. Services will now start automatically on every boot."
echo "Useful commands:"
for service_name in "${!PROJECTS[@]}"; do
  echo "  systemctl status $service_name.service"
  echo "  journalctl -u $service_name.service -f"
done
