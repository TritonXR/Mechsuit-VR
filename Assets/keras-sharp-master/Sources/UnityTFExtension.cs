//This is modified from KerasSharp repo for use of Unity., by Xiaoxiao Ma, Aalto University, 
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System;
public static class TensorFlowSharpEx
{


    // Returns range(0, rank(x)) if reduction_indices is null
    public static TFOutput ReduceDims(this TFGraph g, TFOutput input, TFOutput? axis = null)
    {
        if (axis.HasValue)
            return axis.Value;

        // Fast path: avoid creating Rank and Range ops if ndims is known.
        long[] shape = g.GetTensorShape(input).ToArray();
        if (shape.Length >= 0)
        {
            // The python code distinguishes between tensor and sparsetensor

            var array = new int[shape.Length];
            for (int i = 0; i < array.Length; i++)
                array[i] = i;

            return g.Const(array, TFDataType.Int32);
        }
        return g.Range(g.Const(0), g.Const(shape.Length), g.Const(1));
    }
    
}