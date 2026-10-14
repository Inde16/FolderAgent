# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FolderAgent is a single-file C# console app that implements a conversational AI agent for working with local folders. It connects to a local Ollama instance (qwen3:8b model) and maintains a multi-turn chat loop with conversation history as context.

## Build & Run

```bash
dotnet build
dotnet run
```

Requires Ollama running locally on `http://localhost:11434` with the `qwen3:8b` model pulled (`ollama pull qwen3:8b`).

Target framework: .NET 10.0.

## Architecture

The entire application is in `Program.cs` using C# top-level statements. Key elements:

- **Chat loop**: REPL that reads user input, appends to conversation history, sends full history to Ollama `/api/chat` endpoint, and prints the response.
- **Conversation memory**: A `List<ChatMessage>` that accumulates all messages (system + user + assistant). The full history is sent on every request — this is how the agent "remembers" context.
- **Ollama integration**: Direct HTTP calls via `HttpClient` to the Ollama chat API. No SDK or framework dependencies — just `System.Net.Http.Json`.
- **Records**: `ChatRequest`, `ChatMessage`, `ChatResponse` are minimal records for JSON serialization. `Think: false` disables Ollama's chain-of-thought output.

## Branches

- `main` — current working version (chat loop)
- `ChatLoopMemory` — variant with memory features
