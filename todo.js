// TODO : https://github.com/yargs/yargs

const chalk = require('chalk')
const rl = require('readline')
const low = require('lowdb')
const FileSync = require('lowdb/adapters/FileSync')

const args = process.argv

const adapter = new FileSync('db.json')
const db = low(adapter)
// Set some defaults (required if your JSON file is empty)
db.defaults({ todos: [] }).write()

function prompt(question) {
	const r = rl.createInterface({
		input: process.stdin,
		output: process.stdout,
		terminal: false
	})
	return new Promise((resolve, error) => {
		r.question(question, (answer) => {
			r.close()
			resolve(answer)
		})
	})
}

function newTodo() {
	const q = chalk.blue('Type in your todo\n')
	prompt(q).then((todo) => {
		// add todo
		db
			.get('todos')
			.push({
				title: todo,
				complete: false
			})
			.write()
	})
}

function getTodos() {
	const todos = db.get('todos').value()
	let index = 1
	todos.forEach((todo) => {
		const todoText = `${index++}. ${todo.title}`
		console.log(todoText)
	})
}

const usage = function() {
	const usageText = `
    todo helps you manage you todo tasks.
  
    usage:
      todo <command>
  
      commands can be:
  
      new:      used to create a new todo
      get:      used to retrieve your todos
      complete: used to mark a todo as complete
      help:     used to print the usage guide
    `

	console.log(usageText)
}

function errorLog(error) {
	const eLog = chalk.red(error)
	console.log(eLog)
}

if (args.length > 3 && args[2] != 'complete') {
	errorLog('only one argument can be accepted')
	usage()
	return
}

switch (args[2]) {
	case 'help':
		usage()
		break
	case 'new':
		newTodo()
		break
	case 'get':
		getTodos()
		break
	case 'complete':
		break
	default:
		errorLog('invalid command passed')
		usage()
}
