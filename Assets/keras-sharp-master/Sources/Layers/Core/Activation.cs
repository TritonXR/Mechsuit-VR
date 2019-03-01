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
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using KerasSharp.Activations;
    using KerasSharp.Engine.Topology;

    /// <summary>
    ///   Applies an activation function to an output.
    /// </summary>
    /// 
    /// <seealso cref="KerasSharp.LayerBase" />
    /// 
    [DataContract]
    public class Activation : Layer
    {
        private IActivationFunction activation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Activation"/> class.
        /// </summary>
        /// 
        /// <param name="activation">The activation function.</param>
        /// 
        public Activation(IActivationFunction activation)
        {
            this.supports_masking = true;
            this.activation = activation;
        }

        /*public Activation(string name)
        {
            this.activation = Create(name);
        }*/

        /*public static IActivationFunction Create(string name)
        {
            // https://github.com/fchollet/keras/blob/f65a56fb65062c8d14d215c9f4b1015b97cc5bf3/keras/activations.py#L90
            Type type = typeof(IActivationFunction);
            Type activationType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface)
                .Where(p => p.Name.ToUpperInvariant() == name.ToUpperInvariant())
                .FirstOrDefault();

            if (activationType == null)
                throw new ArgumentOutOfRangeException("name", $"Could not find activation function '{name}'.");

            var activationFunction = (IActivationFunction)Activator.CreateInstance(activationType);
            return activationFunction;
        }*/

        public enum ActivationFunction
        {
            ReLU,
            ELU,
            Sigmoid,
            Swish,
            None
        }
        public static IActivationFunction GetActivationFunction(ActivationFunction activationFunction)
        {
            IActivationFunction result;
            switch (activationFunction)
            {
                case ActivationFunction.ELU:
                    result = new ELU();
                    break;
                case ActivationFunction.ReLU:
                    result = new ReLU();
                    break;
                case ActivationFunction.Sigmoid:
                    result = new Sigmoid();
                    break;
                case ActivationFunction.None:
                    result = null;
                    break;
                case ActivationFunction.Swish:
                    result = new Swish();
                    break;
                default:
                    result = new ReLU();
                    break;
            }
            return result;
        }
        protected override List<Tensor> InnerCall(List<Tensor> inputs, List<Tensor> mask = null, bool? training = null)
        {
            return activation.Call(inputs, mask);
        }

    }
}