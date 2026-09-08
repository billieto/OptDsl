# OptDsl

## Overview

OptDsl is a Domain-Specific Language that lets you choose and parametrize the optimizations of your code. It works with the new pass manager of llvm and specifically with the `opt` tool. The pipeline goes as follows:

```mermaid
flowchart TD
    %% Frontend / Middle-end
    DSL["DSL Script (.optd)"] --> PARSER["ANTLR4 Parser<br>(Lexer & CST)"]
    PARSER --> AST["AST Builder & Semantic Analyzer<br>(Checks against Pass Registry)"]
    AST --> GEN["Pipeline Generator<br>(Extracts TargetConfigs)"]
    %% Target generation split
    GEN --> T1["Target 1<br>(foo.c + pipeline string)"]
    GEN --> T2["Target 2<br>(bar.c + pipeline string)"]
    %% File 1 Pipeline
    subgraph "Execution: File 1"
        T1 --> C1["clang -S -emit-llvm<br>-O0 (Disable default opts)"]
        C1 --> IR1["foo.ll<br>(Raw LLVM IR)"]
        IR1 --> O1["opt -passes='...' <br>(Apply DSL pipeline)"]
        O1 --> OIR1["foo_opt.ll<br>(Optimized IR)"]
    end
    %% File 2 Pipeline
    subgraph "Execution: File n"
        T2 --> C2["clang -S -emit-llvm<br>-O0 (Disable default opts)"]
        C2 --> IR2["bar.ll<br>(Raw LLVM IR)"]
        IR2 --> O2["opt -passes='...' <br>(Apply DSL pipeline)"]
        O2 --> OIR2["bar_opt.ll<br>(Optimized IR)"]
    end
    %% Linking phase
    OIR1 --> LINK["clang<br>(Linker Phase)"]
    OIR2 --> LINK
    
    LINK --> EXE["Final Executable<br>(a.out)"]
    %% Styling
    classDef file fill:#f9f6e8,stroke:#d4c485,stroke-width:2px,color:#333;
    classDef tool fill:#e1f0fa,stroke:#7eb6d9,stroke-width:2px,color:#333;
    
    class DSL,IR1,IR2,OIR1,OIR2,EXE file;
    class PARSER,AST,GEN,C1,C2,O1,O2,LINK tool;
```

- `samples/`: contains all the files generated for the main process above (raw ir, opt ir, executable).
- `debug/`: contains the `.dot` files so the AST and CST can be visualized for debugging purposes.
- `scripts/`: contains bash scripts that automatically do things such as the making of the graphs and deep cleaning the project.

## Requirements

You must have dotnet, llvm, clang and opt tools installed. If you want to visualize the graphs you must have graphviz tool installed as well.

## Usage

In every situation there is a `Properties/` directory that has a json so you can change the source file of type .optd. Dotnet automatically detects it and runs the project with the specifications of the file.

For CLI use it as this at the root level:

``` bash
dotnet run --project src/OptDsl
```

For any IDE just press F5. For VSCode users there is a `.vscode/` directory, but it does the same thing as mentioned.
