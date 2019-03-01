﻿//This is modified from KerasSharp repo for use of Unity., by Xiaoxiao Ma, Aalto University, 
//
// Keras-Sharp: C# port of the Keras library
// https://github.com/cesarsouza/keras-sharp
//
// Based under the Keras library for Python. See LICENSE text for more details.
//
//    The MIT License(MIT)
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//    
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

namespace KerasSharp.Initializers
{
    using KerasSharp.Engine.Topology;
    using System.Runtime.Serialization;

    /// <summary>
    ///   Glorot uniform initializer, also called Xavier uniform initializer.
    /// </summary>
    /// 
    /// <remarks>
    ///  It draws samples from a uniform distribution within [-limit, limit] where <c>limit</c> is 
    ///  <c>sqrt(6 / fan_in)</c> where <c>fan_in</c> is the number of input units in the weight tensor.
    /// </remarks>
    /// 
    [DataContract]
    public class GlorotUniform : IWeightInitializer
    {
        private int? seed;
        private float scale;
        /// <summary>
        /// Initializes a new instance of the <see cref="GlorotUniform"/> class.
        /// </summary>
        /// 
        /// <param name="seed">The integer used to seed the random generator.</param>
        /// 
        public GlorotUniform(int? seed = null, float scale = 1)
        {
            this.seed = seed;
            this.scale = scale;
        }

        /// <summary>
        /// Creates a <see cref="TFTensor" /> with the desired initial weights.
        /// </summary>
        /// 
        /// <param name="shape">The shape of the tensor to be generated.</param>
        /// <param name="dtype">The <see cref="TFDataType">data type</see> of the tensor to be generated.</param>
        /// <returns>A <see cref="TFTensor" /> initialized of dimensions <paramref name="shape" />
        /// and element data type <paramref name="dtype" /> that has been initialized using this
        /// strategy.</returns>
        /// 
        public Tensor Call(int[] shape, DataType? dtype = null)
        {
            return new VarianceScaling(scale: this.scale, mode: "fan_avg", distribution: "uniform", seed: seed).Call(shape, dtype);
        }
    }

}