import React, {useEffect, useState} from 'react';
import styles from '../stylesheets/Search.module.css';
import axios from "axios";

const basePath = '/api';

function Search(props) {
	const [selectedCategory, setSelectedCategory] = useState({
		name: '',
		data: []
	});

	const [categories, setCategories] = useState({
		loading: true,
		data: [],
		error: false
	});

	useEffect(() => {
		axios
			.get(`${basePath}/category`)
			.then(data => {
				setCategories({
					loading: false,
					data: data.data,
					error: false
				})
			})
			.catch(error => {
				setCategories({
					loading: false,
					data: [],
					error: true
				})
			})
	}, [basePath])

	useEffect(() => {
		if (selectedCategory.name.trim().length <= 0) {
			return;
		}
		
		axios
			.get(`${basePath}/category/${selectedCategory.name}`)
			.then(data => {
				setSelectedCategory(prev => {
					return {
						name: prev.name,
						data: data.data
					}
				})
			})
			
	}, [basePath, selectedCategory.name])
	
	if (categories.loading) {
		return <div className={styles.errorTitle}>
			<h1>Loading...</h1>
		</div>
	}
	
	if (categories.error) {
		return <div className={styles.errorTitle}>
			<h1>Uh oh! There was an error loading the page</h1>
			<h3>Please try again later</h3>
		</div>
	}
	
	const handleCategoryChange = (category) => {
		if (selectedCategory.name !== category) {
			setSelectedCategory({
				name: category,
				data: []
			});
		}
	}
	
	return (
		<div className={styles.container}>
			<h1 className={styles.title}>Categories</h1>
			<div className={styles.categories}>
				{
					categories.data.map(category =>
						<span key={category.name} className={`${styles.category} ${selectedCategory.name === category && styles.categorySelected}`} onClick={(event) => handleCategoryChange(category)}>
							{category}
						</span>
					)
				}
			</div>
			
			<div className={styles.apis}>
				{
					selectedCategory.data.map(api => 
						<div key={api.name} className={styles.api}>
							<h2>{api.name}</h2>
							<h3>{api.description}</h3>
							<p><a href={api.link} target={'_blank'} rel="noreferrer">API Link</a></p>
						</div>
					)
				}
			</div>
			
		</div>
	);
}

export default Search;