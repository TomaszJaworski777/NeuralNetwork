using MatrixLib;
using System;

namespace AI
{
    class NeuralNetwork
    {
        //defining layers
        private int inputLayer;
        private int hiddenLayer;
        private int outputLayer;

        //defining matrixes
        private Matrix weights_ih;
        private Matrix weights_ho;
        private Matrix bias_h;
        private Matrix bias_o;

        /// <summary>
        /// Creates instance of Neural Network.
        /// </summary>
        /// <param name="layers">Array of neurons in layers.</param>
        public NeuralNetwork(int[] layers)
        {
            //rewrite params to variables
            inputLayer = layers[0];
            hiddenLayer = layers[1];
            outputLayer = layers[2];

            //initialize weights matrixes
            weights_ih = new Matrix(hiddenLayer, inputLayer);
            weights_ho = new Matrix(outputLayer, hiddenLayer);

            //randomize weights matrixes
            weights_ih.Randomize();
            weights_ho.Randomize();

            //initialize bias matrixes
            bias_h = new Matrix(hiddenLayer, 1);
            bias_o = new Matrix(outputLayer, 1);

            //randomize bias matrixes
            bias_h.Randomize();
            bias_o.Randomize();

        }

        NeuralNetwork(NeuralNetwork network)
        {

        }

        /// <summary>
        /// Pushing data to neural network with using Feed Forward algorithm.
        /// </summary>
        /// <param name="input">One dimentional array of inputs.</param>
        /// <returns>One dimentional array of outputs from neural network.</returns>
        public float[] FeedForward(float[] input)
        {
            //multiplying an weights array by inputs -- dot product
            var hidden = Matrix.Multiply(weights_ih, Matrix.FromArray(input));

            //adding bias to hidden layer values
            hidden.Add(bias_h);

            //mapping ActivationFucntion on hidden layer
            hidden.Map(ActivationFunction);

            //multiplying an weights array by hidden layer values -- dot product
            var output = Matrix.Multiply(weights_ho, hidden);

            //adding bias to output layer
            output.Add(bias_o);

            //mapping ActivationFucntion on output layer
            output.Map(ActivationFunction);


            //returning output layer converted to array
            return output.ToArray();
        }

        /// <summary>
        /// Training algorithm.
        /// </summary>
        /// <param name="input">One dimentional array of inputs.</param>
        /// <param name="target">One dimentonal array of desired outputs.</param>
        /// <param name="learningRate">Learning speed scaler.</param>
        public void BackPropagation(float[] input, float[] target, float learningRate = 0.1f)
        {
            //convert input array to matrix
            var inputs = Matrix.FromArray(input);

            //multiplying an weights array by inputs -- dot product
            var hidden = Matrix.Multiply(weights_ih, inputs);

            //adding bias to hidden layer values
            hidden.Add(bias_h);

            //mapping ActivationFucntion on hidden layer
            hidden.Map(ActivationFunction);

            //multiplying an weights array by hidden layer values -- dot product
            var outputs = Matrix.Multiply(weights_ho, hidden);

            //adding bias to output layer
            outputs.Add(bias_o);

            //mapping ActivationFucntion on output layer
            outputs.Map(ActivationFunction);


            //conver target array to matrix
            var targets = Matrix.FromArray(target);

            //calculating output error matrix by subtracting outputs from targets
            var outputErrors = Matrix.Subtract(targets, outputs);

            //calculating gradients by mapping outputs by SigmoidDerivative function
            var gradients = Matrix.Map(outputs, SigmoidDerivative);

            //multiplying gradients by output errors
            gradients.Multiply(outputErrors);

            //multiplying gradients by learning rate
            gradients.Multiply(learningRate);


            //transpose hidden layer, becouse back propagation is from outputs to inputs
            var hiddenT = Matrix.Transpose(hidden);

            //calculation delta beetween hidden layer and outputs
            var deltaWHO = Matrix.Multiply(gradients, hiddenT);


            //mutate weights beetween hidden and output by delta
            weights_ho.Add(deltaWHO);

            //mutate bias beetween hidden and output by gradient
            bias_o.Add(gradients);


            //transpose weights layer to calculate hidden error
            var transposedHO = Matrix.Transpose(weights_ho);

            //calculating hidden error matrix by multiplying transposed weights and output errors
            var hiddenErrors = Matrix.Multiply(transposedHO, outputErrors);

            //calculating hidden gradients by mapping hidden layer by SigmoidDerivative function
            var hiddenGradient = Matrix.Map(hidden, SigmoidDerivative);

            //multiplying hidden gradients by hidden errors
            hiddenGradient.Multiply(hiddenErrors);

            //multiplying hidden gradients by learning rate
            hiddenGradient.Multiply(learningRate);


            //transpose input layer to calculate delta beetween input and hidden layer
            var inputT = Matrix.Transpose(inputs);

            //calculating delta beetween input and hidden layer by multiplying hidden gradients by transposed inputs
            var deltaWIH = Matrix.Multiply(hiddenGradient, inputT);


            //mutate weights beetween input and hidden layer by delta
            weights_ih.Add(deltaWIH);

            //mutate bias beetween input and hidden layer by hidden gradient
            bias_h.Add(hiddenGradient);
        }

        /// <summary>
        /// Creates copy of this neural network and returns it.
        /// </summary>
        public NeuralNetwork Copy() => new NeuralNetwork(this);

        //universal function to call activation key
        float ActivationFunction(float x)
        {
            return Sigmoid(x);
        }

        float Sigmoid(float x)
        {
            return 1f / (1 + (float)Math.Exp(-x));
        }

        float SigmoidDerivative(float x)
        {
            return x * (1 - x); // sigmoid(x) * (1 - sigmoid(x)) but output is already sigmoid
        }
    }
}