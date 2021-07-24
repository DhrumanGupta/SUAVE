import React, {useEffect, useState} from 'react';
import styles from '../stylesheets/Search.module.css';

function Search(props) {
	// const categories
	
	const [categories, setCategories] = useState({
		loading: false,
		data: [],
		error: false
	});
	
	useEffect(() => {
		
	}, [])
	

	return (
		<div className={styles.container}>
			
			<div className={styles.categories}>
				{
					
				}
			</div>
			
		</div>
	);
}

export default Search;