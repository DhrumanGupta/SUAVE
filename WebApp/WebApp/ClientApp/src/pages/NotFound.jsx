import React from 'react';
import styles from '../stylesheets/NotFound.module.css';
import {Link} from "react-router-dom";

function NotFound(props) {
	return (
		<div className={styles.container}>
			<h1 className={styles.header}>
				404 Not Found
			</h1>
			
			<h3 className={styles.subheader}>
				The page you requested was not found.
				<br/>
				<Link to={'/'}>Return to Home</Link>
			</h3>
		</div>
	);
}

export default NotFound;