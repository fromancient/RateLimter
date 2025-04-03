# Multi-Threaded Rate Limiter: Implementation and Design Choices

## Overview
This repository contains a **Rate Limiter** library implementation with a **Token Bucket Strategy**, which was chosen because it efficiently manages request bursts while enforcing long-term rate limits. Unlike rigid fixed-window approaches, it allows temporary bursts within defined limits before throttling requests. The project includes a sample project demonstrating its usage and unit tests to validate its correctness. The project is structured into three main components:

1. **RateLimiter Library** - Implements rate limiting strategies.
2. **ExampleProject** - Demonstrates how to integrate the rate limiter.
3. **RateLimiterTests** - Unit tests ensuring the reliability of the implementation.

## Features
- Implements **Token Bucket Strategy** for rate limiting.
- Thread-safe implementation for handling concurrent requests.
- Support for **custom rate limits** (per second, minute, or day).
- Middleware integration support.
- Comprehensive unit tests.

## Project Structure
```
RateLimiter
│── src
│   │── ExampleProject/              # Example usage of Rate Limiter
│   │   ├── Program.cs
│   │   ├── Startup.cs
│   │   ├── WeatherForecast.cs
│   │── RateLimiter/                 # Core Rate Limiter Library
│   │   ├── Cache/
│   │   ├── Core/
│   │   │   ├── IRateLimiter.cs
│   │   │   ├── RateLimiterService.cs
│   │   ├── Middleware/
│   │   │   ├── RateLimiterMiddleware.cs
│   │   │   ├── RateLimiterMiddlewareExtensions.cs
│   │   ├── Strategy/
│   │   │   ├── IRateLimiterStrategy.cs
│   │   │   ├── TokenBucketStrategy.cs
│   │   │   ├── TokenBucketRecord.cs
│── tests
│   │── RateLimiterTests/             # Unit tests for Rate Limiter
│   │   ├── TokenBucketStrategyTests.cs
```

## Installation
Clone the repository and navigate to the project folder:
```sh
$ git clone https://github.com/fromancient/RateLimiter.git
$ cd RateLimiter
```

Restore dependencies:
```sh
$ dotnet restore
```

## Usage
### Using the Rate Limiter in an Application
1. Add the **RateLimiter** library to your project.
2. Implement and configure the **Token Bucket Strategy**.

Example setup in `Startup.cs`:
```csharp
var rateLimiter = new RateLimiterService(new TokenBucketStrategy(new InMemoryCache()), 10, TimeSpan.FromSeconds(1));
services.AddSingleton<IRateLimiter>(rateLimiter);
```

Applying the rate limiter to API requests:
```csharp
app.UseMiddleware<RateLimiterMiddleware>();
```

### Running the Example Project
```sh
$ cd src/ExampleProject
$ dotnet run
```

### Running Unit Tests
```sh
$ cd tests/RateLimiterTests
$ dotnet test
```