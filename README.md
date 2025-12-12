# Advent of Code

This repository will contain all my solutions to Advent of Code puzzles from every year. I decided to make a repository with all my Advent of Code years so they could all be in one place, and so I could run each of them easier. The code uses `.NET 10.0`.

- Use `dotnet run today` to solve the current day's puzzle
- Use `dotnet run -year -day` to solve a specific day's puzzle (e.g `dotnet run 2025 08`)
  - You can also use `dotnet run -year` to solve every puzzle from a specific year
- Use `dotnet run create -year -day` to create a folder and `Solution.cs` file (e.g `dotnet run create 2025 09`)
- Use `dotnet run all` to solve every puzzle

## Storing your session cookie

You can optionally store your AoC session cookie as an environment variable, so the program can automatically get your input when you run the create command.

**Windows**:

- Run `setx ADVENT_OF_CODE_SESSION "your-session-cookie"`
- Open a new terminal or restart IDE for changes to take effect
- Confirm the environment variable has been set
  - Command Prompt: Run `echo %ADVENT_OF_CODE_SESSION%`
  - PowerShell: Run `echo $env:ADVENT_OF_CODE_SESSION`

**Mac/Linux**:

- For the current terminal session only, run: `export ADVENT_OF_CODE_SESSION="your-session-cookie"`
- To make it permanent, add the line to your shell config file (~/.bashrc or ~/.zshrc):
  `echo 'export ADVENT_OF_CODE_SESSION="your-session-cookie"' >> ~/.bashrc`
- Then you can reload with: `source ~/.bashrc`
- Run `echo $ADVENT_OF_CODE_SESSION` to confirm the environment variable has been set

> Replace `your-session-cookie` with the session cookie stored in your browser on the AoC website.

⚠ Do NOT share your session cookie. It grants full access to your Advent of Code account. Keep it private. This repository only uses the session cookie to load your input into a file for you automatically when you run the create command.

## Credit

I modeled this repository based off another one that I saw on GitHub.

https://github.com/encse/adventofcode

https://adventofcode.com/
