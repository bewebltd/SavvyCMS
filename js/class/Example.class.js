// @class
function Example() {
	var self=this;
	// @private
	var privateField = 5;

	// @public
	self.init = init;
	function init() {
		// This is a public method	
		//privateField
		calculate(); //good
	};

	self.test = function () {
		// This is a public method	 - need to use this. (self.) in the function to get to ,e,ber vars
		//self.privateField		ok
		//self.calculate()
		calculate();//
	};

	// @private
	//self.calculate = calculate; //uncomment to make public
	function calculate() {
		// This is a private method
		//$('div').each(function (index,el) {
			//$(this) //different this
		//});
	};

};

// Singleton pattern - Construct whenever the script loads, you can call init in your document.ready event
window.example = new Example();
//example.init();			//ok
//example.test();			//ok
//example.privateField //bad
//example.calculate //bad


/*
// Declaring our Animal object
var Animal = function () {
    this.name = 'unknown';
    this.getName = function () {
       return this.name;
   }
    return this;
};

// Declaring our Dog object
var Dog = function () {
    // A private variable here        
    var private = 42;
    // overriding the name
    this.name = "Bello";
    // Implementing ".bark()"
    this.bark = function () {
        return 'WPPF';
    }  
    return this;
};

// Dog extends animal
Dog.prototype = new Animal();			 //*** important

// -- Done declaring --

// Creating an instance of Dog.
var dog = new Dog();

// Proving our case
console.log(
    "Is dog an instance of Dog? ", dog instanceof Dog, "\n",
    "Is dog an instance of Animal? ", dog instanceof Animal, "\n",
    dog.bark() +"\n", // Should be: "WPPF"
    dog.getName() +"\n", // Should be: "Bello"
    dog.private +"\n" // Should be: 'undefined'
);


var dog4 = new Dog();
*/
