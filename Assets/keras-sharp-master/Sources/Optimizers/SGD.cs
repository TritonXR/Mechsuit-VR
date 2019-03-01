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
    using KerasSharp.Constraints;
    using KerasSharp.Engine.Topology;
    using KerasSharp.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using static Backends.Current;


    /// <summary>
    ///   Stochastic gradient descent optimizer.
    /// </summary>
    /// 
    /// <remarks>
    ///  Includes support for momentum, learning rate decay, and Nesterov momentum.
    /// </remarks>
    /// 
    /// <seealso cref="KerasSharp.Models.IOptimizer" />
    /// 
    [DataContract]
    public class SGD : OptimizerBase, IOptimizer
    {
        private Tensor iterations;
        private Tensor lr;
        private Tensor momentum;
        private Tensor decay;
        private double initial_decay;
        private bool nesterov;

        /// <summary>
        /// Initializes a new instance of the <see cref="SGD" /> class.
        /// </summary>
        /// 
        /// <param name="lr">float >= 0. Learning rate.</param>
        /// <param name="momentum">float >= 0. Parameter updates momentum.</param>
        /// <param name="decay">float >= 0. Learning rate decay over each update.</param>
        /// <param name="nesterov">Whether to apply Nesterov momentum.</param>
        /// 
        public SGD(double lr = 0.01, double momentum = 0.0, double decay = 0.0, bool nesterov = false)
            : base()
        {
            // https://github.com/fchollet/keras/blob/f65a56fb65062c8d14d215c9f4b1015b97cc5bf3/keras/optimizers.py#L144

            this.iterations = K.variable(0, name: "iterations");
            this.lr = K.variable(lr, name: "lr");
            this.momentum = K.variable(momentum, name: "momentum");
            this.decay = K.variable(decay, name: "decay");
            this.initial_decay = decay;
            this.nesterov = nesterov;
        }

        public override List<List<Tensor>> get_updates(List<Tensor> param, Dictionary<Tensor, IWeightConstraint> constraints, Tensor loss)
        {
            using (K.name_scope($"SGD"))
            {
                var grads = this.get_gradients(loss, param);
                this.updates = new List<List<Tensor>>();

                if (this.initial_decay > 0)
                    this.lr *= (1 / (1 + this.decay * this.iterations));

                this.updates.Add(new List<Tensor> { K.update_add(this.iterations, 1f, name: "iterations/update") });

                // momentum
                List<Tensor> moments;

                using (K.name_scope("moments"))
                {
                    List<int?[]> shapes = param.Select(p => K.get_variable_shape(p)).ToList();
                    moments = shapes.Select(s => K.zeros(s)).ToList();
                }

                this.Weights = new[] { this.iterations }.Concat(moments).ToList();

                for (int i = 0; i < param.Count; i++)
                {
                    using (K.name_scope($"{param[i].name}"))
                    {
                        Tensor p = param[i];
                        Tensor g = grads[i];
                        Tensor m = moments[i];

                        Tensor v = this.momentum * m - lr * g;  // velocity

                        this.updates.Add(new List<Tensor> { K.update(m, v, "momentum/update") });

                        Tensor new_p;
                        if (this.nesterov)
                            new_p = p + this.momentum * v - lr * g;
                        else
                            new_p = p + v;

                        // apply constraints
                        if (constraints != null && constraints.ContainsKey(p))
                            new_p = constraints[p].Call(new_p);

                        updates.Add(new List<Tensor> { K.update(p, new_p, "parameter/update") });
                    }
                }

                return this.updates;
            }
        }



        public override void SetLearningRate(float lr)
        {
            K.set_value(this.lr, new float[] { lr });
        }

    }
}