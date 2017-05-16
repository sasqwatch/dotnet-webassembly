﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAssembly
{
	/// <summary>
	/// The data section declares the initialized data that is loaded into the linear memory.
	/// </summary>
	public class Data
	{
		/// <summary>
		/// The linear memory index (always 0 in the initial version of WebAssembly).
		/// </summary>
		public uint Index { get; set; }

		private IList<Instruction> initializerExpression;

		/// <summary>
		/// An <see cref="ValueType.Int32"/> initializer expression that computes the offset at which to place the data.
		/// </summary>
		/// <exception cref="ArgumentNullException">Value cannot be set to null.</exception>
		public IList<Instruction> InitializerExpression
		{
			get => this.initializerExpression ?? (this.initializerExpression = new List<Instruction>());
			set => this.initializerExpression = value ?? throw new ArgumentNullException(nameof(value));
		}

		private IList<byte> rawData;

		/// <summary>
		/// Raw data in byte form.
		/// </summary>
		/// <exception cref="ArgumentNullException">Value cannot be set to null.</exception>
		public IList<byte> RawData
		{
			get => this.rawData ?? (this.rawData = new List<byte>());
			set => this.rawData = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Creates a new <see cref="Data"/> instance.
		/// </summary>
		public Data()
		{
		}

		internal Data(Reader reader)
		{
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));

			this.Index = reader.ReadVarUInt32();
			this.initializerExpression = Instruction.ParseInitializerExpression(reader).ToArray();
			this.rawData = reader.ReadBytes(reader.ReadVarUInt32());
		}

		/// <summary>
		/// Expresses the value of this instance as a string.
		/// </summary>
		/// <returns>A string representation of this instance.</returns>
		public override string ToString() => $"Index: {Index}, Length: {rawData?.Count()}";
	}
}