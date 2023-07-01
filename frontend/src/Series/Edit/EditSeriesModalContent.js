import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { inputTypes, kinds } from 'Helpers/Props';
import Button from 'Components/Link/Button';
import SpinnerButton from 'Components/Link/SpinnerButton';
import ModalContent from 'Components/Modal/ModalContent';
import ModalHeader from 'Components/Modal/ModalHeader';
import ModalBody from 'Components/Modal/ModalBody';
import ModalFooter from 'Components/Modal/ModalFooter';
import Form from 'Components/Form/Form';
import FormGroup from 'Components/Form/FormGroup';
import FormLabel from 'Components/Form/FormLabel';
import FormInputGroup from 'Components/Form/FormInputGroup';

import MoveSeriesModal from 'Series/MoveSeries/MoveSeriesModal';
import styles from './EditSeriesModalContent.css';

class EditSeriesModalContent extends Component {

  //
  // Lifecycle

  constructor(props, context) {
    super(props, context);

    this.state = {
      isConfirmMoveModalOpen: false
    };
  }

  //
  // Listeners

  onSavePress = () => {
    const {
      isPathChanging,
      onSavePress
    } = this.props;

    if (isPathChanging && !this.state.isConfirmMoveModalOpen) {
      this.setState({ isConfirmMoveModalOpen: true });
    } else {
      this.setState({ isConfirmMoveModalOpen: false });

      onSavePress(false);
    }
  }

  onMoveSeriesPress = () => {
    this.setState({ isConfirmMoveModalOpen: false });

    this.props.onSavePress(true);
  }

  //
  // Render

  render() {
    const {
      item,
      isSaving,
      showLanguageProfile,
      originalPath,
      onInputChange,
      onModalClose,
      onDeleteSeriesPress,
      ...otherProps
    } = this.props;

    const {
      monitored,
      seasonFolder,
      qualityProfileId,
      languageProfileId,
      seriesType,
      path,
      title,
      tags
    } = item;

    return (
      <ModalContent onModalClose={onModalClose}>
        <ModalHeader>
          Edit - {otherProps.title}
        </ModalHeader>

        <ModalBody>
          <Form {...otherProps}>
            <FormGroup>
              <FormLabel>Title</FormLabel>
              <FormInputGroup
                type={inputTypes.TEXT}
                value= {otherProps.title}
                name="title"
                helpText="You can add translations(Scene Naming Exceptions) in this entry, if your translation did not exist in sonarr, send it to the sonar community with the google form!"
                {...title}
                onChange={onInputChange}
              />
             
            </FormGroup>
            <FormGroup>
              <FormLabel>Sonarr Scene Naming Exceptions + link to add google forms</FormLabel>
              {otherProps.alternateTitlesLinks.map((object, i) => <div key={object.key}><span>{object.name} </span><a href={object.url} target="_blank">[link google form]</a></div>)}
             
              <div> 
                <span>
                  <a href="https://docs.google.com/spreadsheets/d/1PiIvzijwcdALKQWfGE3j4lwnOqmDkhB48fyQTArJpI4/edit#gid=675284162" target="_blank">Excel Naming Exception</a>
                </span>
              </div>
            </FormGroup>
            <FormGroup>
              <FormLabel>Monitored</FormLabel>

              <FormInputGroup
                type={inputTypes.CHECK}
                name="monitored"
                helpText="Download monitored episodes in this series"
                {...monitored}
                onChange={onInputChange}
              />
            </FormGroup>

            <FormGroup>
              <FormLabel>Use Season Folder</FormLabel>

              <FormInputGroup
                type={inputTypes.CHECK}
                name="seasonFolder"
                helpText="Sort episodes into season folders"
                {...seasonFolder}
                onChange={onInputChange}
              />
            </FormGroup>

            <FormGroup>
              <FormLabel>Quality Profile</FormLabel>

              <FormInputGroup
                type={inputTypes.QUALITY_PROFILE_SELECT}
                name="qualityProfileId"
                {...qualityProfileId}
                onChange={onInputChange}
              />
            </FormGroup>

            {
              showLanguageProfile &&
                <FormGroup>
                  <FormLabel>Language Profile</FormLabel>

                  <FormInputGroup
                    type={inputTypes.LANGUAGE_PROFILE_SELECT}
                    name="languageProfileId"
                    {...languageProfileId}
                    onChange={onInputChange}
                  />
                </FormGroup>
            }

            <FormGroup>
              <FormLabel>Series Type</FormLabel>

              <FormInputGroup
                type={inputTypes.SERIES_TYPE_SELECT}
                name="seriesType"
                {...seriesType}
                onChange={onInputChange}
              />
            </FormGroup>

            <FormGroup>
              <FormLabel>Path</FormLabel>

              <FormInputGroup
                type={inputTypes.PATH}
                name="path"
                {...path}
                onChange={onInputChange}
              />
            </FormGroup>

            <FormGroup>
              <FormLabel>Tags</FormLabel>

              <FormInputGroup
                type={inputTypes.TAG}
                name="tags"
                {...tags}
                onChange={onInputChange}
              />
            </FormGroup>
          </Form>
        </ModalBody>

        <ModalFooter>
          <Button
            className={styles.deleteButton}
            kind={kinds.DANGER}
            onPress={onDeleteSeriesPress}
          >
            Delete
          </Button>

          <Button
            onPress={onModalClose}
          >
            Cancel
          </Button>

          <SpinnerButton
            isSpinning={isSaving}
            onPress={this.onSavePress}
          >
            Save
          </SpinnerButton>
        </ModalFooter>

        <MoveSeriesModal
          originalPath={originalPath}
          destinationPath={path.value}
          isOpen={this.state.isConfirmMoveModalOpen}
          onSavePress={this.onSavePress}
          onMoveSeriesPress={this.onMoveSeriesPress}
        />
      </ModalContent>
    );
  }
}

EditSeriesModalContent.propTypes = {
  seriesId: PropTypes.number.isRequired,
  title: PropTypes.string.isRequired,
  item: PropTypes.object.isRequired,
  isSaving: PropTypes.bool.isRequired,
  showLanguageProfile: PropTypes.bool.isRequired,
  isPathChanging: PropTypes.bool.isRequired,
  originalPath: PropTypes.string.isRequired,
  onInputChange: PropTypes.func.isRequired,
  onSavePress: PropTypes.func.isRequired,
  onModalClose: PropTypes.func.isRequired,
  onDeleteSeriesPress: PropTypes.func.isRequired
};

export default EditSeriesModalContent;
