import _ from 'lodash';
import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { connect } from 'react-redux';
import { createSelector } from 'reselect';
import { saveSeries, setSeriesValue } from 'Store/Actions/seriesActions';
import createSeriesSelector from 'Store/Selectors/createSeriesSelector';
import selectSettings from 'Store/Selectors/selectSettings';
import EditSeriesModalContent from './EditSeriesModalContent';

function createIsPathChangingSelector() {
  return createSelector(
    (state) => state.series.pendingChanges,
    createSeriesSelector(),
    (pendingChanges, series) => {
      const path = pendingChanges.path;

      if (path == null) {
        return false;
      }

      return series.path !== path;
    }
  );
}

function createMapStateToProps() {
  return createSelector(
    (state) => state.series,
    createSeriesSelector(),
    createIsPathChangingSelector(),
    (seriesState, series, isPathChanging) => {
      const {
        isSaving,
        saveError,
        pendingChanges
      } = seriesState;

      const seriesSettings = _.pick(series, [
        'monitored',
        'monitorNewItems',
        'seasonFolder',
        'qualityProfileId',
        'seriesType',
        'path',
        'tags',
        'title'
      ]);

      const settings = selectSettings(seriesSettings, pendingChanges, saveError);
      var alternateTitlesLinks = series.alternateTitles.map(function(item, key) {
        var url = "https://docs.google.com/forms/d/e/1FAIpQLSdn_l_RvNCAPdL-ahVV0Z9hHw1swcxMLrEPY3pJE9qa7IR3tw/viewform?usp=pp_url" + "&entry.2173272=" + series.tvdbId + "&entry.826062213=" + item.title + "&entry.474981866=" + series.title + "&entry.1231195804=Spanish%20translate";
        return {
            key: key,
            url: url,
            name: item.title
        };
      }, "");
      return {
        title: series.title,
        tvdbId: series.tvdbId,
        alternateTitlesLinks: alternateTitlesLinks,
        isSaving,
        saveError,
        isPathChanging,
        originalPath: series.path,
        item: settings.settings,
        ...settings
      };
    }
  );
}

const mapDispatchToProps = {
  dispatchSetSeriesValue: setSeriesValue,
  dispatchSaveSeries: saveSeries
};

class EditSeriesModalContentConnector extends Component {

  //
  // Lifecycle

  componentDidUpdate(prevProps, prevState) {
    if (prevProps.isSaving && !this.props.isSaving && !this.props.saveError) {
      this.props.onModalClose();
    }
  }

  //
  // Listeners

  onInputChange = ({ name, value }) => {
    this.props.dispatchSetSeriesValue({ name, value });
  };

  onSavePress = (moveFiles) => {
    this.props.dispatchSaveSeries({
      id: this.props.seriesId,
      moveFiles
    });
  };

  //
  // Render

  render() {
    return (
      <EditSeriesModalContent
        {...this.props}
        onInputChange={this.onInputChange}
        onSavePress={this.onSavePress}
        onMoveSeriesPress={this.onMoveSeriesPress}
      />
    );
  }
}

EditSeriesModalContentConnector.propTypes = {
  seriesId: PropTypes.number,
  isSaving: PropTypes.bool.isRequired,
  saveError: PropTypes.object,
  dispatchSetSeriesValue: PropTypes.func.isRequired,
  dispatchSaveSeries: PropTypes.func.isRequired,
  onModalClose: PropTypes.func.isRequired
};

export default connect(createMapStateToProps, mapDispatchToProps)(EditSeriesModalContentConnector);
