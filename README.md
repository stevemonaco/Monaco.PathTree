[![NuGet](https://img.shields.io/nuget/v/Monaco.PathTree.svg?style=flat)](https://www.nuget.org/packages/Monaco.PathTree/)
![NetStandard2.1](https://badgen.net/badge/Framework/.NET&nbsp;Standard&nbsp;2.1/blue)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/stevemonaco/Monaco.PathTree/blob/master/LICENSE)
[![ci](https://github.com/stevemonaco/Monaco.PathTree/actions/workflows/ci.yml/badge.svg)](https://github.com/stevemonaco/Monaco.PathTree/actions/workflows/ci.yml)

# Monaco.PathTree
Generic tree structure accessed through disk-like paths. Developed in C# as a .NET Standard 2.1 library.

# What is a path?
A path is a string delimited by certain character(s) where each item specifies the next level of the tree. The leading and trailing separators 
are not enforced. By default, the separators are `/` and `\`. Only complete, absolute paths are currently supported. The following are some valid example paths:

`/Root/Folder1/ResourceA`

`Root\Folder1\ResourceA`

# Major components

#### `PathTree<TNode, TItem>`
Tree which provides access to nodes through absolute paths.

#### `PathNode<TItem>`
Node which provides access to the parent node, children nodes, and an associated Item.

#### `Extensions`
Several tree and node extension methods are available for the following traversal operations: breadth-first traversal, depth-first traversal, ancestors, and descendents.

# User-defined extensibility via Monaco.PathTree.Abstractions

The interfaces `IPathTree<TNode, TItem>` and `IPathNode<TNode, TItem>` define the basic contract.

The default implementation is provided through the abstract classes `PathTreeBase<TNode, TItem>` and `PathNodeBase<TNode, TItem>`. The built-in types derive directly from these with no changes besides the simpler type syntax for `PathNode<TItem>`.

# Sample App
Simple CLI program that demonstrates how to programmatically build the tree using: 1. the built-in types 2. user-defined types through `Monaco.PathTree.Abstractions`. Then prints the tree out to console.
