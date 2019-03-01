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


namespace KerasSharp.Optimizers
{
    using Accord.Math;
    using KerasSharp.Constraints;
    using KerasSharp.Engine.Topology;
    using KerasSharp.Losses;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using static Backends.Current;


    /// <summary>
    ///   Abstract optimizer base class.
    /// </summary>
    /// 
    /// <seealso cref="KerasSharp.Models.IOptimizer" />
    /// 
    [DataContract]
    public abstract class OptimizerBase
    {
        protected List<List<Tensor>> updates;

        /// <summary>
        /// The weight tensors of the optimizer
        /// </summary>
        public List<Tensor> Weights { get; protected set; }
        public double clipnorm;
        public double clipvalue;

        protected OptimizerBase()
        {
            //var allowed_kwargs = new[] { "clipnorm", "clipvalue" };

            //foreach (var k in kwargs)
            //    if (!allowed_kwards.Contains(k))
            //        throw new Exception("Unexpected keyword argument passed to optimizer: " + k);
            // this.__dict__.update(kwargs)

            this.updates = new List<List<Tensor>>();
            this.Weights = new List<Tensor>();
        }

        public virtual List<List<Tensor>> get_updates(List<Tensor> collected_trainable_weights, Dictionary<Tensor, IWeightConstraint> constraints, Tensor total_loss)
        {
            throw new NotImplementedException();
        }

        public List<Tensor> get_gradients(Tensor loss, List<Tensor> param)
        {
            List<Tensor> grads = K.gradients(loss, param);

            if (this.clipnorm > 0)
            {
                var norm = K.sqrt(K.sum(grads.Select(g => K.sum(K.square(g))).ToList()));
                grads = grads.Select(g => K.clip_norm(g, this.clipnorm, norm)).ToList();
            }

            if (clipvalue > 0)
                grads = grads.Select(g => K.clip(g, -this.clipvalue, this.clipvalue)).ToList();

            return grads;
        }

        /// <summary>
        ///   Sets the weights of the optimizer, from Numpy arrays.
        /// </summary>
        /// 
        /// <remarks>
        ///  Should only be called after computing the gradients (otherwise the optimizer has no weights).
        /// </remarks>
        /// 
        /// <param name="weights">The list of Numpy arrays. The number of arrays and their shape must match 
        ///   number of the dimensions of the weights of the optimizer(i.e.it should match the output of 
        ///   <see cref="get_weights"/></param>
        /// 
        public void set_weights(List<Array> weights)
        {
            var param = this.Weights;
            var weight_value_tuples = new List<ValueTuple<Tensor, Array>>();
            var param_values = K.batch_get_value(param);

            for (int i = 0; i < param_values.Count; i++)
            {
                Array pv = param_values[i];
                Tensor p = param[i];
                Array w = weights[i];

                if (pv.GetLength().IsEqual(w.GetLength()) && !(pv.Rank == 1 && pv.Length == w.Length))
                    throw new Exception($"Optimizer weight shape {pv.GetLength()} not compatible with provided weight shape {w.GetLength()}.");

                weight_value_tuples.Add(ValueTuple.Create(p, w));
            }

            K.batch_set_value(weight_value_tuples);
        }

        /// <summary>
        ///   Returns the current value of the weights of the optimizer.
        /// </summary>
        public List<Array> get_weights()
        {
            return K.batch_get_value(this.Weights);
        }

        public abstract void SetLearningRate(float lr);
    }
}