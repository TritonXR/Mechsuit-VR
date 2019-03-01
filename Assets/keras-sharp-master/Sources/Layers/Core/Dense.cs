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


namespace KerasSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Accord.Math;
    using KerasSharp.Constraints;
    using KerasSharp.Engine.Topology;
    using KerasSharp.Initializers;
    using KerasSharp.Regularizers;
    using static Backends.Current;
    using KerasSharp.Backends;

    /// <summary>
    ///   Just your regular densely-connected NN layer.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   <c>Dense</c> implements the operation: <c>output = activation(dot(input, kernel) + bias)</c>
    ///    where <c>activation</c> is the element-wise activation function passed as the <c>activation</c> 
    ///    argument, <c>kernel</c> is a weights matrix created by the layer, and `bias` is a bias vector 
    ///    created by the layer (only applicable if <see cref="use_bias"/> is <c>true</c>).</para>
    ///    
    /// <para>
    ///    Note: if the input to the layer has a rank greater than 2, then it is flattened prior to the 
    ///    initial dot product with `kernel`.</para>
    /// </remarks>
    /// 
    /// <seealso cref="KerasSharp.LayerBase" />
    /// <seealso cref="KerasSharp.ILayer" />
    /// 
    [DataContract]
    public class Dense : Layer
    {

        public Tensor kernel;
        public Tensor bias;

        public int units;
        public int input_dim;
        public IActivationFunction activation;
        public bool use_bias;
        public IWeightInitializer kernel_initializer;
        public IWeightInitializer bias_initializer;
        public IWeightRegularizer kernel_regularizer;
        public IWeightRegularizer bias_regularizer;
        public IWeightConstraint kernel_constraint;
        public IWeightConstraint bias_constraint;

        public int spectral_norm_iteration = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dense"/> class.
        /// </summary>
        /// 
        /// <param name="units">Positive integer, dimensionality of the output space.</param>
        /// <param name="input_dim">The input dim.</param>
        /// <param name="batch_input_shape">The batch input shape.</param>
        /// <param name="input_shape">The input shape.</param>
        /// <param name="activation">The activation function to use.</param>
        /// <param name="use_bias">Whether the layer uses a bias vector.</param>
        /// <param name="spectral_norm_iteration">Whether the layer uses a bias vector.</param>
        /// 
        public Dense(int units, IActivationFunction activation = null, bool use_bias = true,
            IWeightInitializer kernel_initializer = null, IWeightInitializer bias_initializer = null,
            IWeightRegularizer kernel_regularizer = null, IWeightRegularizer bias_regularizer = null, IWeightRegularizer activity_regularizer = null,
            IWeightConstraint kernel_constraint = null, IWeightConstraint bias_constraint = null,
            int? input_dim = null, int?[] input_shape = null, int?[] batch_input_shape = null, int spectral_norm_iteration = 0)
            : base(input_dim: input_dim, input_shape: input_shape, batch_input_shape: batch_input_shape)
        {
            // https://github.com/fchollet/keras/blob/f65a56fb65062c8d14d215c9f4b1015b97cc5bf3/keras/layers/core.py#L791

            //if 'input_shape' not in kwargs and 'input_dim' in kwargs:
            //    kwargs['input_shape'] = (kwargs.pop('input_dim'),)

            if (bias_initializer == null)
                bias_initializer = new Zeros();
            if (kernel_initializer == null)
                kernel_initializer = new GlorotUniform();

            this.units = units;
            this.activation = activation;
            this.use_bias = use_bias;
            this.kernel_initializer = kernel_initializer;
            this.bias_initializer = bias_initializer;
            this.kernel_regularizer = kernel_regularizer;
            this.bias_regularizer = bias_regularizer;
            this.activity_regularizer = activity_regularizer;
            this.kernel_constraint = kernel_constraint;
            this.bias_constraint = bias_constraint;

            this.input_spec = new List<InputSpec>();
            this.input_spec.Add(new InputSpec(min_ndim: 2));
            this.supports_masking = true;
            this.spectral_norm_iteration = spectral_norm_iteration;
        }

        /*/// <summary>
        /// Initializes a new instance of the <see cref="Dense"/> class.
        /// </summary>
        /// 
        /// <param name="units">Positive integer, dimensionality of the output space.</param>
        /// <param name="input_dim">The input dim.</param>
        /// <param name="batch_input_shape">The batch input shape.</param>
        /// <param name="input_shape">The input shape.</param>
        /// <param name="activation">The activation function to use.</param>
        /// <param name="use_bias">Whether the layer uses a bias vector.</param>
        /// 
        public Dense(int units, string activation, bool use_bias = true,
            IWeightInitializer kernel_initializer = null, IWeightInitializer bias_initializer = null,
            IWeightRegularizer kernel_regularizer = null, IWeightRegularizer bias_regularizer = null, IWeightRegularizer activity_regularizer = null,
            IWeightConstraint kernel_constraint = null, IWeightConstraint bias_constraint = null,
            int? input_dim = null, int?[] input_shape = null, int?[] batch_input_shape = null)
            : this(units: units, activation: Activation.Create(activation), use_bias: use_bias,
                   kernel_initializer: kernel_initializer, bias_initializer: bias_initializer,
                   kernel_regularizer: kernel_regularizer, bias_regularizer: bias_regularizer, activity_regularizer: activity_regularizer,
                   kernel_constraint: kernel_constraint, bias_constraint: bias_constraint,
                   input_dim: input_dim, input_shape: input_shape, batch_input_shape: batch_input_shape)
        {
        }*/

        protected override void build(List<int?[]> input_shape)
        {
            // https://github.com/fchollet/keras/blob/f65a56fb65062c8d14d215c9f4b1015b97cc5bf3/keras/layers/core.py#L818

            if (input_shape[0].Length < 2)
                throw new ArgumentException("input_shape");

            int input_dim = Matrix.Get(input_shape[0], -1).Value;

            this.kernel = add_weight(shape: new int[] { input_dim, this.units },
                initializer: this.kernel_initializer,
                regularizer: this.kernel_regularizer,
                constraint: this.kernel_constraint,
                name: "kernel");

            if (this.use_bias)
            {
                this.bias = base.add_weight(shape: new int[] { this.units },
                    name: "bias",
                    initializer: bias_initializer,
                    regularizer: bias_regularizer,
                    constraint: bias_constraint);
            }
            else
            {
                this.bias = null;
            }

            this.input_spec = new List<InputSpec>() { new InputSpec(min_ndim: 2, axes: new Dictionary<int, int>() { { -1, input_dim } }) };
            this.built = true;
        }


        protected override Tensor InnerCall(Tensor inputs, Tensor mask = null, bool? training = null)
        {
            // https://github.com/fchollet/keras/blob/2382f788b4f14646fa8b6b2d8d65f1fc138b35c4/keras/layers/core.py#L840
            Tensor output = null;
            if(spectral_norm_iteration <= 0)
                output = K.dot(inputs, this.kernel, name: null);
            else
                output = K.dot(inputs, K.spectral_norm(this.kernel,spectral_norm_iteration), name: null);


            if (this.use_bias)
                output = K.bias_add(output, this.bias, name: null);
            if (this.activation != null)
                output = this.activation.Call(output, mask);
            return output;
        }

        public override List<int?[]> compute_output_shape(List<int?[]> input_shapes)
        {
            if (input_shapes.Count != 1)
                throw new Exception("Expected a single input.");
            int?[] input_shape = input_shapes[0];

            if (input_shape.Length < 2)
                throw new Exception("Shape should contain at least a batch size and number of dimensions.");
            if (Matrix.Get(input_shape, -1) <= 0)
                throw new Exception();

            int?[] output_shape = input_shape.Copy();
            output_shape.Set(index: -1, value: this.units);
            return new[] { output_shape }.ToList();
        }

    }
}