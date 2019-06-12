const rabbot = require('rabbot')
const chalk = require('chalk')

// TODO : Take from environment variables
const settings = {
	connection: {
		user: 'admin',
		pass: 'pass',
		server: 'localhost',
		vhost: 'k8s-demo',
		port: 5673,
		timeout: 1000,
		failAfter: 30,
		retryLimit: 400
	},
	exchanges: [
		{ name: 'dead-letter-ex', type: 'fanout' },
		{ name: 'driver.hired', type: 'topic' },
		{ name: 'driver.status-update', type: 'topic' },
		{ name: 'package.added', type: 'topic' },
		{ name: 'package.warehouse-scan', type: 'topic' },
		{ name: 'package.driver-scan', type: 'topic' }
	],
	queues: [
		{ name: 'dead-letter-q' },
		{ name: 'driver.hired.service', deadLetter: 'dead-letter-ex' },
		{ name: 'driver.status-update.load-van.service', deadLetter: 'dead-letter-ex' },
		{ name: 'driver.status-update.run-started.service', deadLetter: 'dead-letter-ex' },
		{ name: 'driver.status-update.run-completed.service', deadLetter: 'dead-letter-ex' },
		{ name: 'package.added.service', deadLetter: 'dead-letter-ex' },
		{ name: 'package.warehouse-scan.hip.service', deadLetter: 'dead-letter-ex' },
		{ name: 'package.warehouse-scan.dip.service', deadLetter: 'dead-letter-ex' },
		{ name: 'package.driver-scan.vop.service', deadLetter: 'dead-letter-ex' },
		{ name: 'package.driver-scan.dnc.service', deadLetter: 'dead-letter-ex' },
		{ name: 'package.driver-scan.pod.service', deadLetter: 'dead-letter-ex' },
		{ name: 'package.driver-scan.vip.service', deadLetter: 'dead-letter-ex' }
	],
	bindings: [
		{ exchange: 'dead-letter-ex', target: 'dead-letter-q' },
		{ exchange: 'driver.hired', target: 'driver.hired.service', keys: [ '*' ] },
		{ exchange: 'driver.status-update', target: 'driver.status-update.load-van.service', keys: [ 'load-van' ] },
		{ exchange: 'driver.status-update', target: 'driver.status-update.run-started.service', keys: [ 'run-started' ] },
		{
			exchange: 'driver.status-update',
			target: 'driver.status-update.run-completed.service',
			keys: [ 'run-complete' ]
		},
		{ exchange: 'package.added', target: 'package.added.service', keys: [ '*' ] },
		{ exchange: 'package.warehouse-scan', target: 'package.warehouse-scan.hip.service', keys: [ 'hip' ] },
		{ exchange: 'package.warehouse-scan', target: 'package.warehouse-scan.dip.service', keys: [ 'dip' ] },
		{ exchange: 'package.driver-scan', target: 'package.driver-scan.vop.service', keys: [ 'vop' ] },
		{ exchange: 'package.driver-scan', target: 'package.driver-scan.dnc.service', keys: [ 'dnc' ] },
		{ exchange: 'package.driver-scan', target: 'package.driver-scan.pod.service', keys: [ 'pod' ] },
		{ exchange: 'package.driver-scan', target: 'package.driver-scan.vip.service', keys: [ 'vip' ] }
	]
}

rabbot
	.configure(settings)
	.then(() => {
		console.log(chalk.green('Setup 🔥'))
		process.exit(0)
	})
	.catch((err) => {
		console.error(chalk.red('Failed to Setup 🤢'))
		console.error(chalk.red(err))
		process.exit(-1)
	})
