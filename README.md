# Simple Note Taking Application 

## Table of Contents
- [Project Description](#project-description)
- [Building and Running](#building-and-running)
- [HowToUse](#how-to-use)
- [DemoVideo](#demo-video)
- [Testing](#testing)
- [FutureImprovements](#future-improvements)

# Project Description
This is a simple command-line note-taking app that provides the essential functionality for creating and managing notes.  

It allows users to:
- **Create** notes with a title and content  
- **List** all notes, displaying their titles  
- **View** a specific note to see its title and content  
- **Delete** notes by their title  

### Bonus Features
In addition, bonus features are also added:
- **Edit**: update the content of existing notes  
- **Search**: find notes by matching text in the title or content, with results displayed in a table format  
- **Timestamps**: each note records when it was created and last modified  
- **Colored output**: command results are color-coded for clarity:  
  - Green = Success  
  - Yellow = Invalid Input  
  - Red = Error  


# Building and Running

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- (Alternative) [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **.NET desktop development** workload installed.

## Build

### Directly from terminal
1. Navigate to the project root (`SimpleNoteTakingApp`).  
2. Build the project in **Release** mode:  

        dotnet build -c Release --project App/SimpleNoteTakingApp.csproj

3. Run the app directly from the terminal:  

        dotnet run -c Release --project App/SimpleNoteTakingApp.csproj

4. Alternatively, run the compiled executable directly:  

        App\bin\Release\net8.0\SimpleNoteTakingApp.exe

### Using Visual Studio

1. Open the solution in the project root or the project file `App/SimpleNoteTakingApp.csproj` in Visual Studio.  
2. Set the build configuration to **Release** and run the application.
3. A terminal window will open with the app ready for input.  

# How to Use
Once running, the application starts in interactive mode. Type commands at the prompt (`>`).  

Commands:  
- `add "<title>" "<content>"` -> creates and add a new note  
- `list` -> list all notes with their title and content preview. 
- `view "<title>"` -> display full details of a note  
- `delete "<title>"` -> remove a note  
- `edit "<title>" "<new content>"` -> update a note’s content  
- `search "<query>"` -> find notes by matching text in the title or content  
- `help` -> show the list of commands  
- `quit` / `exit` -> close the application  

Example usage:
help

add "Today's task" "Check out content on C# .net"

list

view "Today's task"

edit "Today's task" "Learn how to use .net"

search ".net"

delete "Today's task"

exit

# Demo video

# Testing
This project includes unit tests written with **xUnit** to validate functionality across the application.  

The tests cover:  
- **Command parsing** -> ensuring valid commands are accepted and invalid ones are rejected  
- **Error handling** -> verifying that incorrect input returns the appropriate error messages and result types  
- **Core logic** -> testing note creation, editing, deleting, and searching, as well as the internal `NoteManager` to ensure correct storage and retrieval behavior  

### Running the Tests
From the project root, you can run tests in either of the following ways:

1. Run directly at the project root:  

        dotnet test ./SimpleNoteTakingApp.Tests

2. Or navigate into the test project from the project root and run:  

        cd SimpleNoteTakingApp.Tests
        dotnet test



# Future Improvements
While the current implementation meets the requirements of the assignment, there are a few limitations and areas that could be improved:

1. **Duplicate titles**: Currently, note titles must be unique. A potential improvement would be to assign each note a unique ID while still allowing duplicate titles. This would make the app more flexible, with IDs stored alongside titles for identification.

2. **Lookup performance**: For this assignment, a simple list was chosen for note storage, prioritising clarity and ease of implementation.

This works well, but as the collection grows, lookups (`get`, `edit`, `delete`) may become slower (`O(n)`).

Inspired by how databases optimise queries, potential improvements include:
   - A simple approach would be to maintain a dictionary that caches note indices by title (`Dictionary<string, int>` or similar) for `O(1)` average-case lookups.  
   - A more advance technique would be caching frequently accessed notes, since each note already contains timestamps that could support an LRU-style policy.
