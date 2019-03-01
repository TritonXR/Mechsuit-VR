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

namespace KerasSharp.Losses
{
    using KerasSharp.Engine.Topology;
    using System;
    using System.Runtime.Serialization;

    using static Backends.Current;

    /// <summary>
    ///   Binary Cross Entropy loss.
    /// </summary>
    /// 
    /// <seealso cref="KerasSharp.Losses.ILoss" />
    /// 
    [DataContract]
    public class BinaryCrossEntropy : ILoss
    {

        /// <summary>
        ///   Wires the given ground-truth and predictions through the desired loss.
        /// </summary>
        /// 
        /// <param name="expected">The ground-truth data that the model was supposed to approximate.</param>
        /// <param name="actual">The actual data predicted by the model.</param>
        /// 
        /// <returns>A scalar value representing how far the model's predictions were from the ground-truth.</returns>
        /// 
        public Tensor Call(Tensor expected, Tensor actual, Tensor sample_weight = null, Tensor mask = null)
        {
            if (sample_weight != null || mask != null)
                throw new NotImplementedException();

            using (K.name_scope("binary_cross_entropy"))
                return K.mean(K.binary_crossentropy(output: actual, target: expected), axis: -1);
        }
    }
}